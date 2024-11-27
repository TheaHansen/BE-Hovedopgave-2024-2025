using BE_Hovedopgave_2024_2025.Model;
using Microsoft.EntityFrameworkCore;

namespace BE_Hovedopgave_2024_2025.Services;

public class LabelService : ILabelService
{
    
    private readonly OdontologicDbContext _context;
    
    public LabelService(OdontologicDbContext context)
    {
        _context = context;
    }
    
    public async Task<Label?> GetLabelByNameAsync(string name)
    {
        var labelEntity = await _context.Labels
            .FirstOrDefaultAsync(l => l.Name == name);

        return labelEntity;
    }
}