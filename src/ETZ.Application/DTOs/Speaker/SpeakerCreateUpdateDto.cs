namespace ETZ.Application.DTOs.Speaker;

public sealed class SpeakerCreateUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string SpeakerPhotoUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}


