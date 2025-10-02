using ETZ.Domain.Entities;

namespace ETZ.Application.DTOs.Speaker;

public sealed class SpeakerDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string SpeakerPhotoUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public List<SpeakerTranslationDto> Translations { get; set; } = new();
}


