using ETZ.Domain.Entities;
namespace ETZ.Application.DTOs.Material;

public class MaterialInformationDto
{
    public Guid Id { get; set; }
    public string MaterialName { get; set; } = null!;
    public string MaterialUrl { get; set; } = null!;
    public string MaterialType { get; set; } = null!;
    public string PlacementCode { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int DisplayOrder { get; set; }
}
