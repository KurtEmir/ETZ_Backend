namespace ETZ.Application.DTOs.Topic;

public sealed class TopicInformationDto
{
    public Guid Id { get; set; }
    public string TopicName { get; set; } = null!;
    public int DisplayOrder { get; set; }
}


