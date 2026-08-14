
using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.data;
using backend.middleware;
using backend.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace backend.controller;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    private readonly ILogger<InventoryController> _logger;
    private readonly IMapper _mapper;
    private readonly AppDataContext _context;

    public InventoryController(
        ILogger<InventoryController> logger,
        IMapper mapper,
        AppDataContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }

    /// <summary>
    /// Adds a product to user
    /// </summary>
    [Authorize]
    [HttpPost("product")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProductInventoryResponseDTO>> AddProduct([FromBody] ProductInventoryRequestDTO productInventoryRequestDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }   

        try
        {
            if (await _context.Products.AnyAsync(u => u.ProductName == productInventoryRequestDTO.ProductName))
            {
                _logger.LogWarning($"Product Creation attempt with existing name: {productInventoryRequestDTO.ProductName}");
                return Conflict(new { message = "Product Name is already taken. " });
            }

            var newProduct = _mapper.Map<Product>(productInventoryRequestDTO);
            newProduct.CreatedAt = DateTime.UtcNow;
            newProduct.LastUpdatedAt = DateTime.UtcNow; 

            // Associate product with the current authenticated user
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("Attempt to create product without authenticated user");
                return Unauthorized(new { message = "User must be authenticated to create a product." });
            }

            newProduct.UserId = userId;

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"New Product registered: {newProduct.ProductName}");

            var response = _mapper.Map<ProductInventoryResponseDTO>(newProduct);
            return CreatedAtAction(nameof(GetProduct), new { productName = newProduct.ProductName }, response);
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during product creation");
            Console.WriteLine($"PRODUCT CREATION ERROR: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred during product creation.", error = ex.Message });
        }

        return StatusCode(StatusCodes.Status501NotImplemented); //Delete this after your done with said method
    }

    /// <summary>
    /// Creates a sale
    /// </summary>
    [HttpGet("{productName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductInventoryResponseDTO>> GetProduct(string productName)
    {
        try
        {
            var product = await _context.Products
            .Where(u => u.ProductName == productName)
            .ProjectTo<ProductSalesResponseDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

            if (product == null)
            {
                _logger.LogWarning("Product with {ProductName} does not exist", productName);
                return NotFound(new { message = "Product Not Found" });
            }

            return Ok(_mapper.Map<ProductInventoryRequestDTO>(product));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting user: {productName}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }




    /// <summary>
    /// Creates a sale
    /// </summary>
    [HttpPost("sale")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProductSalesResponseDTO>> CreateSale([FromBody] CreateSaleRequestDTO createSaleRequestDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            //Code here

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during sale creation");
            Console.WriteLine($"SALE CREATION ERROR: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred during sale creation.", error = ex.Message });
        }

        return StatusCode(StatusCodes.Status501NotImplemented); //Delete this after your done with said method
    }

    /// <summary>
    /// Views all the products
    /// </summary>
    [HttpGet("products")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<ProductInventoryResponseDTO>>> GetAllProducts()
    {
        try
        {
            //Code here

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all products");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return StatusCode(StatusCodes.Status501NotImplemented); //Delete this after your done with said method
    }

    /// <summary>
    /// View product by its category
    /// </summary>
    [HttpGet("products/{category}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProductInventoryResponseDTO>>> GetProductsByCategory(string category)
    {
        try
        {
            //Code here

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all products");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return StatusCode(StatusCodes.Status501NotImplemented); //Delete this after your done with said method
    }

    /// <summary>
    /// View possible sales to be made
    /// </summary>
    [HttpGet("sales")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<ProductSalesResponseDTO>>> GetAvailableSales()
    {
        try
        {
            //Code here
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all products");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return StatusCode(StatusCodes.Status501NotImplemented); //Delete this after your done with said method
    }

    /// <summary>
    /// View possible Sale based on category
    /// </summary>
    [HttpGet("sales/{category}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProductSalesResponseDTO>>> GetSalesByCategory(string category)
    {
        try
        {
            //Code here
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all products");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return StatusCode(StatusCodes.Status501NotImplemented); //Delete this after your done with said method
    }

    /// <summary>
    /// Delete Product by ID
    /// </summary>
    [HttpDelete("product/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            //Code here

        }

        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting product with ID: {id}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while deleting the product.", error = ex.Message });
        }

        return StatusCode(StatusCodes.Status501NotImplemented); //Delete this after your done with said method
    }
}

//uh goodluck ig HAHAHAHAH