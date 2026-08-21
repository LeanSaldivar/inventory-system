
using System.Collections;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.data;
using backend.middleware;
using backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            if (!Enum.IsDefined(productInventoryRequestDTO.ProductCategory))
            {
                _logger.LogWarning(
                    "Product creation attempt with invalid category: {Category}",
                    productInventoryRequestDTO.ProductCategory);

                return BadRequest(new { message = "Invalid product category." });
            }

            if (!Enum.IsDefined(productInventoryRequestDTO.ProductUnit))
            {
                _logger.LogWarning(
                    "Product creation attempt with invalid unit: {Category}",
                    productInventoryRequestDTO.ProductCategory);

                return BadRequest(new { message = "Invalid product unit." });
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
    }

    /// <summary>
    /// View a product
    /// </summary>
    [HttpGet("{productName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductInventoryResponseDTO>> GetProduct(string productName)
    {
        try
        {
            var product = await _context.Products
            .ProjectTo<ProductInventoryResponseDTO>(_mapper.ConfigurationProvider)
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
            _logger.LogError(ex, $"Error getting product: {productName}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
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
            var products = await _context.Products
            .ProjectTo<ProductInventoryResponseDTO>(_mapper.ConfigurationProvider)
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
            if (!Enum.TryParse<ProductCategory>(category, true, out var productCategory))
            {
                return BadRequest("Invalid product category.");
            }

            var products = await _context.Products
            .Where(u => u.ProductCategory == productCategory)
            .ProjectTo<ProductInventoryResponseDTO>(_mapper.ConfigurationProvider)
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
            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                _logger.LogWarning($"Attempt to delete non-existet product with ID: {id}");
                return NotFound(new { message = "User Not Found" });
            }

            _context.Products.Remove(products);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Product deleted: {products.ProductName}");
            return NoContent();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting product with ID: {id}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while deleting the product.", error = ex.Message });
        }

    }
}