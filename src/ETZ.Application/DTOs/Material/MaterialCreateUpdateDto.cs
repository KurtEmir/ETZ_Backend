using ETZ.Domain.Entities;

namespace ETZ.Application.DTOs.Material;
public class MaterialCreateUpdateDto
{
    public string? MaterialName { get; set; }
    public string MaterialUrl { get; set; } = null!;
    public MaterialType MaterialType { get; set; }
    public string PlacementCode { get; set; } = null!;
    public int? DisplayOrder { get; set; }
    public List<MaterialTranslationDto> Translations { get; set; } = new();
}

public sealed class MaterialTranslationDto
{
    public LanguageCode LanguageCode { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}