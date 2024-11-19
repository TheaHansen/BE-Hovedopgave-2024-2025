using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Hovedopgave_2024_2025.Model;

public class Stock
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    
    public int ColourId { get; set; }
    
    public int SizeId { get; set; }
    
    public int Quantity { get; set; }
    
}