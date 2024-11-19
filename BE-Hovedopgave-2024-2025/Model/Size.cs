using System.Text.Json.Serialization;

namespace BE_Hovedopgave_2024_2025.Model;

public class Size
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public List<Stock> Stocks { get; set; }

}