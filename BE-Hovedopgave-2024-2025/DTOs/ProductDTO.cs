using System.ComponentModel.DataAnnotations;

namespace BE_Hovedopgave_2024_2025.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; }
    [MaxLength(41)]
    public string? ShortDescription { get; set; }
    [MaxLength(255)]
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public List<LabelDTO> Labels { get; set; } = [];
    public string StockStatus { get; set; }
}