namespace BE_Hovedopgave_2024_2025.Model;

public class Label
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; } = [];

}