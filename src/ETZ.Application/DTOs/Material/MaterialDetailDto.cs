using ETZ.Domain.Entities;

namespace ETZ.Application.DTOs.Material;

public sealed class MaterialDetailDto
{
    public Guid Id { get; set; }
    public string? MaterialName { get; set; }
    public string MaterialUrl { get; set; } = string.Empty;
    public MaterialType MaterialType { get; set; }
    public string PlacementCode { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public List<MaterialTranslationDto> Translations { get; set; } = new();
}


