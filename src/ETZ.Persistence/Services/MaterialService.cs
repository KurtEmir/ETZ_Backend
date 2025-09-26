using ETZ.Persistence.Context;
using Microsoft.Extensions.Logging;
using ETZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ETZ.Application.DTOs.Material;
using ETZ.Application;
namespace ETZ.Persistence.Services;

public sealed class MaterialService
{
    private readonly ETZDbContext _context;
    private readonly ILogger<MaterialService> _logger;

    public MaterialService(ETZDbContext context, ILogger<MaterialService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Material>> GetAllAsync()
        => await _context.Materials.AsNoTracking().Where(x => !x.IsDeleted)
        .Include(x => x.MaterialContents.Where(mc => !mc.IsDeleted))
        .Include(x => x.MaterialPlacements)
        .Include(x => x.MaterialType)
        .OrderBy(x => x.DisplayOrder).ToListAsync();

    public async Task<Material?> GetByIdAsync(Guid id)
        => await _context.Materials.AsNoTracking().Where(x => !x.IsDeleted && x.Id == id)
        .Include(x => x.MaterialContents.Where(mc => !mc.IsDeleted))
        .Include(x => x.MaterialPlacements)
        .Include(x => x.MaterialType)
        .FirstOrDefaultAsync();

    public async Task<Response> CreateAsync(MaterialCreateUpdateDto dto)
    {
        if (dto == null) return Response.Fail("MaterialCreateUpdateDto is required");
        if (string.IsNullOrWhiteSpace(dto.MaterialUrl)) return Response.Fail("MaterialUrl is required");
        if (string.IsNullOrWhiteSpace(dto.PlacementCode)) return Response.Fail("PlacementCode is required");
        if (dto.Translations is null || dto.Translations.Count == 0) return Response.Fail("At least one translation is required");
           
        var material = new Material
        {
            Id = Guid.NewGuid(),
            MaterialName = dto.MaterialName,
            MaterialUrl = dto.MaterialUrl,
            MaterialType = dto.MaterialType,
            DisplayOrder = dto.DisplayOrder ?? 0,
            CreationTime = DateTimeOffset.UtcNow,
            CreatorUserId = Constants.SystemGodUserId,
            IsDeleted = false,
            DeletionTime = null,
            DeleterUserId = null,
            LastModificationTime = null,
            LastModifierUserId = null
        };
        await _context.Materials.AddAsync(material);


        foreach (var t in dto.Translations.GroupBy(t => t.LanguageCode).Select(g => g.First()))
        {
            var content = new MaterialContent
            {
                MaterialId = material.Id,
                LanguageCode = t.LanguageCode,
                Title = t.Title,
                Description = t.Description,
                CreationTime = DateTimeOffset.UtcNow,
                CreatorUserId = Constants.SystemGodUserId,
                IsDeleted = false,
                DeletionTime = null,
                DeleterUserId = null,
                LastModificationTime = null,
                LastModifierUserId = null
            };
            await _context.MaterialContents.AddAsync(content);
        }

        var materialPlacement = new MaterialPlacement   
        {
            MaterialId = material.Id,
            PlacementCode = dto.PlacementCode,
            CreationTime = DateTimeOffset.UtcNow,
            CreatorUserId = Constants.SystemGodUserId,
            IsDeleted = false,
            DeletionTime = null,
            DeleterUserId = null,
            LastModificationTime = null,
            LastModifierUserId = null
        };
        await _context.MaterialPlacements.AddAsync(materialPlacement);
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_MaterialContents_MaterialId_LanguageCode") == true)
        {
            _logger.LogWarning(ex, "Duplicate content for MaterialId={MaterialId}", material.Id);
            return Response.Fail("This material already has content for the given language");
        }
        _logger.LogInformation("Material created Id={Id}", material.Id);
        return Response.Ok("Material created successfully");
    }

    public async Task<List<MaterialInformationDto>> GetMaterialsByLanguage(LanguageCode lang)
    {
        return await _context.Materials
            .AsNoTracking()
            .Where(m => !m.IsDeleted && m.MaterialContents.Any(mc => !mc.IsDeleted && mc.LanguageCode == lang))
            .Include(m => m.MaterialContents.Where(mc => !mc.IsDeleted && mc.LanguageCode == lang))
            .Include(m => m.MaterialPlacements)
            .OrderBy(m => m.DisplayOrder)
            .Select(m => new MaterialInformationDto
            {
                Id = m.Id,
                MaterialName = m.MaterialName ?? string.Empty,
                MaterialUrl = m.MaterialUrl,
                MaterialType = m.MaterialType.ToString(),
                PlacementCode = m.MaterialPlacements.Select(p => p.PlacementCode).FirstOrDefault() ?? string.Empty,
                Title = m.MaterialContents.Where(mc => mc.LanguageCode == lang && !mc.IsDeleted).Select(mc => mc.Title).FirstOrDefault() ?? string.Empty,
                Description = m.MaterialContents.Where(mc => mc.LanguageCode == lang && !mc.IsDeleted).Select(mc => mc.Description).FirstOrDefault() ?? string.Empty,
                DisplayOrder = m.DisplayOrder
            })
            .ToListAsync();
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        if (material is null) return Response.Fail("Material not found");

        material.IsDeleted = true;
        material.DeleterUserId = Constants.SystemGodUserId;
        material.DeletionTime = DateTimeOffset.UtcNow;

        // child records soft delete
        await _context.MaterialContents.Where(c => c.MaterialId == id && !c.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(c => c.IsDeleted, true)
                .SetProperty(c => c.DeletionTime, DateTimeOffset.UtcNow)
                .SetProperty(c => c.DeleterUserId, Constants.SystemGodUserId));

        await _context.MaterialPlacements.Where(p => p.MaterialId == id && !p.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(p => p.IsDeleted, true)
                .SetProperty(p => p.DeletionTime, DateTimeOffset.UtcNow)
                .SetProperty(p => p.DeleterUserId, Constants.SystemGodUserId));

        await _context.SaveChangesAsync();
        _logger.LogInformation("Material deleted Id={Id}", id);
        return Response.Ok("Material deleted successfully");
    }
}

