using BE_Hovedopgave_2024_2025.Model;

namespace BE_Hovedopgave_2024_2025.Services;

public interface ILabelService
{
    public Task<Label?> GetLabelByNameAsync(string name);
}