using ETZ.Domain.Entities;

namespace ETZ.Application.DTOs.Speaker;

public sealed class SpeakerCreateUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string SpeakerPhotoUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public List<SpeakerTranslationDto> Translations { get; set; } = new();
}

public sealed class SpeakerTranslationDto
{
    public LanguageCode LanguageCode { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}


