using AutoMapper;
using BE_Hovedopgave_2024_2025.DTOs;
using BE_Hovedopgave_2024_2025.Enums;
using BE_Hovedopgave_2024_2025.Model;
using Microsoft.EntityFrameworkCore;

namespace BE_Hovedopgave_2024_2025.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly OdontologicDbContext _context;
    
    public ProductService(IMapper mapper, OdontologicDbContext context)
    {
        _mapper = mapper;   
        _context = context;
    }
    
    public StockStatus GetStockStatus(List<Stock> stocks)
    {
        var totalQuantity = stocks.Sum(s => s.Quantity);
        
        if (totalQuantity == 0 || !stocks.Any())
        {
            return StockStatus.OutOfStock;
        }

        return StockStatus.InStock;
    }
    
    public ProductDTO ConvertToProductDTO(Product product)
    {
        
        var stockStatus = GetStockStatus(product.Stocks);
        
        var productDTO = _mapper.Map<ProductDTO>(product);
        
        productDTO.StockStatus = stockStatus.ToString();
        
        return productDTO;
    }

    public async Task<List<ProductDTO>> GetAllProductDTOs()
    {
        var products = await _context.Products
            .Include(p => p.Stocks)
            .ToListAsync();

        var productDTOs = new List<ProductDTO>();

        foreach (var product in products)
        {
            var productDTO = ConvertToProductDTO(product);
            productDTOs.Add(productDTO);
        }
        
        return productDTOs;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var result = await _context.Products
            .Include(p => p.Stocks)
            .FirstOrDefaultAsync(p => p.Id == id);
    
        return result;
    }

    public async Task<List<ProductDTO?>> GetProductDTOsByLabel(string label)
    {
        var products = await _context.Products
            .Include(p => p.Stocks)
            .Where(p => p.Labels.Any(l => l.Name == label))
            .ToListAsync();
        
        var productDTOs = new List<ProductDTO>();
        
        foreach (var product in products)
        {
            var productDTO = ConvertToProductDTO(product);
            productDTOs.Add(productDTO);
        }
        
        return productDTOs;
    }

    public async Task<List<ProductDTO>> GetProductDTOsByCarousel(bool carousel)
    {
        var carouselProducts = await _context.Products.Include(p => p.Stocks).Where(product => product.InCarousel == carousel).ToListAsync();
        
        var productDTOs = new List<ProductDTO>();
        
        foreach (var product in carouselProducts)
        {
            var productDTO = ConvertToProductDTO(product);
            productDTOs.Add(productDTO);
        }
        
        return productDTOs;
    }

    public async Task<List<ProductDTO>> GetProductDTOsByName(string name)
    {
        var searchProducts = await _context.Products
            .Where(p => p.Title.ToLower().Contains(name.ToLower()))
            .ToListAsync();
        
        var productDTOs = new List<ProductDTO>();
        foreach (var product in searchProducts)
        {
            var productDTO = ConvertToProductDTO(product);
            productDTOs.Add(productDTO);
        }

        return productDTOs;
    }

}