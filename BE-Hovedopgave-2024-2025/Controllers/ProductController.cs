using BE_Hovedopgave_2024_2025.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_Hovedopgave_2024_2025.Controllers;

//Made together
[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly OdontologicDbContext _context;

    public ProductController(OdontologicDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }
    
    //Make sure the parameter type is int
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound("Product not found");
        }
        
        return product;
    }

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