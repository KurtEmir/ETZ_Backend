using ETZ.Domain.Entities;
namespace ETZ.Application.DTOs.Speaker;

public class SpeakerContentDto
{
    public Guid SpeakerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public LanguageCode LanguageCode { get; set; }
}