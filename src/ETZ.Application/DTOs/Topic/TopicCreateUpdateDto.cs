using ETZ.Domain.Entities;
namespace ETZ.Application.DTOs.Topic;

public sealed class TopicCreateUpdateDto
{
    public string TopicName { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public LanguageCode LanguageCode { get; set; }
}


