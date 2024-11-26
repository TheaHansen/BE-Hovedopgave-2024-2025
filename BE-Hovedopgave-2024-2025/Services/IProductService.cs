using BE_Hovedopgave_2024_2025.DTOs;
using BE_Hovedopgave_2024_2025.Enums;
using BE_Hovedopgave_2024_2025.Model;

namespace BE_Hovedopgave_2024_2025.Services;

public interface IProductService
{
    public ProductDTO GetProductDto(Product product);
    public Task<Product> GetProductByIdAsync(int id);

    public StockStatus GetStockStatus(List<Stock> stocks);
}