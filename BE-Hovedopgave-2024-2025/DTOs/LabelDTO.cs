namespace BE_Hovedopgave_2024_2025.DTOs;

public class LabelDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ProductDTO> Products { get; set; } = [];
}