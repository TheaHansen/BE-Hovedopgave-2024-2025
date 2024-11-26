using BE_Hovedopgave_2024_2025.DTOs;
using BE_Hovedopgave_2024_2025.Model;
using BE_Hovedopgave_2024_2025.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_Hovedopgave_2024_2025.Controllers;

//Made together
[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly OdontologicDbContext _context;
    
    private readonly IProductService _productService;

    public ProductController(OdontologicDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }
    

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }
    
    //Make sure the parameter type is int
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }
        
        var productDto = _productService.GetProductDto(product);

        return productDto;
    } 
    /*public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound("Product not found");
        }
        
        return product;
    }*/
    
    

    [HttpGet("{label}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByLabel(string label)
    {
        //To see if label exists
        var labelEntity = await _context.Labels
            .FirstOrDefaultAsync(l => l.Name == label);

        if (labelEntity == null)
        {
            return NotFound($"Label '{label}' not found");
        }

        var products = await _context.Products
            .Where(p => p.Labels.Any(l => l.Name == label))
            .ToListAsync();
        if (products.Count == 0)
        {
            return NotFound($"Product not found with '{label}'");
        }

        return products;
    }
}