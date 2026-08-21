using System.Collections;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.controller;
using backend.data;
using backend.middleware;
using backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace backend.Controller;

[ApiController]
[Route("api/sale")]
public class SalesController : ControllerBase
{
    private readonly ILogger<SalesController> _logger;
    private readonly IMapper _mapper;
    private readonly AppDataContext _context;

    public SalesController(
        ILogger<SalesController> logger,
        IMapper mapper,
        AppDataContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }

    /// <summary>
    /// Creates a sale receipt with items
    /// </summary>
    [Authorize]
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ReceiptResponse>> CreateSale([FromBody] ReceiptRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("Attempt to create receipt without authenticated user");
                return Unauthorized(new { message = "User must be authenticated to create a receipt." });
            }

            // Validate all products exist and build receipt items
            var receiptItems = new List<ReceiptItem>();
            decimal subtotal = 0;

            foreach (var item in request.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {item.ProductId} not found");
                    return NotFound(new { message = $"Product with ID {item.ProductId} not found." });
                }

                var itemSubtotal = product.ProductPrice * item.Quantity;
                subtotal += itemSubtotal;

                var receiptItem = new ReceiptItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.ProductPrice,
                    Subtotal = itemSubtotal
                };
                receiptItems.Add(receiptItem);
            }

            // Create receipt
            var receipt = new Receipt
            {
                UserId = userId,
                Subtotal = subtotal,
                Discount = 0,
                Total = subtotal,
                Items = receiptItems,
                CreatedAt = DateTime.UtcNow
            };

            _context.Receipts.Add(receipt);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Receipt created with ID: {receipt.ReceiptId}");

            var response = _mapper.Map<ReceiptResponse>(receipt);
            return CreatedAtAction(nameof(GetReceipt), new { receiptId = receipt.ReceiptId }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during receipt creation");
            Console.WriteLine($"RECEIPT CREATION ERROR: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred during receipt creation.", error = ex.Message });
        }
    }

    /// <summary>
    /// Views all the receipts
    /// </summary>
    [HttpGet("receipt")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ReceiptResponse>> GetAllReceipt()
    {
        try
        {
            var receipt = await _context.Receipts
            .ProjectTo<ReceiptResponse>(_mapper.ConfigurationProvider)
            .ToListAsync();

            return Ok(receipt);

        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error getting all receipts");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpGet("receipt/{receiptId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReceiptResponse>> GetReceipt(int receiptId)
    {
        try
        {
            var receipt = await _context.Receipts
                .Include(r => r.Items)
                .FirstOrDefaultAsync(r => r.ReceiptId == receiptId);

            if (receipt == null)
            {
                _logger.LogWarning($"Receipt with ID {receiptId} not found");
                return NotFound(new { message = "Receipt not found." });
            }

            var response = _mapper.Map<ReceiptResponse>(receipt);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving receipt: {receiptId}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("product/{productName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductSalesResponseDTO>> GetSale(string productName)
    {
        try
        {
            var product = await _context.Products
            .ProjectTo<ProductSalesResponseDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(u => u.ProductName == productName);

            if (product == null)
            {
                _logger.LogWarning("Product with {ProductName} does not exist", productName);
                return NotFound(new { message = "Product Not Found" });
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting sale: {productName}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// View possible sales to be made
    /// </summary>
    [HttpGet("product")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<ProductSalesResponseDTO>>> GetAvailableSales()
    {
        try
        {
            var products = await _context.Products
            .ProjectTo<ProductSalesResponseDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all products");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// View possible Sale based on category
    /// </summary>
    [HttpGet("product/category/{category}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProductSalesResponseDTO>>> GetSalesByCategory(string category)
    {
        try
        {
            if (!Enum.TryParse<ProductCategory>(category, true, out var productCategory))
            {
                return BadRequest("Invalid product category.");
            }

            var products = await _context.Products
             .Where(u => u.ProductCategory == productCategory)
             .ProjectTo<ProductSalesResponseDTO>(_mapper.ConfigurationProvider)
             .ToListAsync();

            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all products");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }





}
