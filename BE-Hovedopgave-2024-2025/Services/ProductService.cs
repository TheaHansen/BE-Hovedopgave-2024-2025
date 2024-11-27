using AutoMapper;
using BE_Hovedopgave_2024_2025.DTOs;
using BE_Hovedopgave_2024_2025.Enums;
using BE_Hovedopgave_2024_2025.Model;
using Microsoft.AspNetCore.Mvc;
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

        if (totalQuantity == 0)
        {
            return StockStatus.OutOfStock;
        }

        return StockStatus.InStock;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var result = await _context.Products.FindAsync(id);
        
        return result;
    }

    public ProductDTO GetProductDto(Product product)
    {
        var stockStatus = GetStockStatus(product.Stocks);
        
        var productDTO = _mapper.Map<ProductDTO>(product);
        
        productDTO.StockStatus = stockStatus.ToString();
        
        return productDTO;
    }
}