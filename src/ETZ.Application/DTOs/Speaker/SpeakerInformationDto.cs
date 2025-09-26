namespace ETZ.Application.DTOs.Speaker;

public class SpeakerInformationDto
{
    public Guid Id { get; set; }
    public string SpeakerName { get; set; } = null!;
    public string SpeakerSurname { get; set; } = null!;
    public string SpeakerTitle { get; set; } = null!;
    public string SpeakerPhotoUrl { get; set; } = null!;
    public int DisplayOrder { get; set; }
}