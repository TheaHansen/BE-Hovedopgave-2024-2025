using System.ComponentModel.DataAnnotations;

namespace BE_Hovedopgave_2024_2025.Model;

public class Product
{
    public int Id { get; set; }
    public string ArticleNumber { get; set; }
    [MaxLength(255)]
    public string Title { get; set; }
    [MaxLength(41)]
    public string? ShortDescription { get; set; }
    [MaxLength(255)]
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public bool InCarousel { get; set; }
    public List<Label> Labels { get; set; } = [];
    public List<Stock> Stocks { get; set; } = [];
}