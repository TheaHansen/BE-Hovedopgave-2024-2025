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
    
    private readonly IProductService _productService;
    private readonly ILabelService _labelService;
    
    public ProductController(IProductService productService, ILabelService labelService)
    {
        _productService = productService;
        _labelService = labelService;
    }
    

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
    {
        return await _productService.GetAllProductDTOs();
    }
    
    //Make sure the parameter type is int
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound("Product not found");
        }
        
        var productDto = _productService.ConvertToProductDTO(product);

        return productDto;
    } 
    

    [HttpGet("{label}")]
    public async Task<ActionResult<IEnumerable<ProductDTO?>>> GetProductsByLabel(string label)
    {
        //To see if label exists
        var labelEntity = await _labelService.GetLabelByNameAsync(label);

        if (labelEntity == null)
        {
            return NotFound($"Label '{label}' not found");
        }

        var productDTOs = await _productService.GetProductDTOsByLabel(label);
        if (productDTOs.Count == 0)
        {
            return NotFound($"Product not found with '{label}'");
        }

        return productDTOs;
    }
}