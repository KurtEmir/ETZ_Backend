using ETZ.Domain.Entities;
namespace ETZ.Application.DTOs.Sponsor;

public class SponsorCreateUpdateDto
{
    public string Name { get; set; } = null!;
    public string SponsorLogoUrl { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public List<SponsorTranslationDto> Translations { get; set; } = new();
}

public sealed class SponsorTranslationDto
{
    public LanguageCode LanguageCode { get; set; }
    public string Type { get; set; } = string.Empty;
}