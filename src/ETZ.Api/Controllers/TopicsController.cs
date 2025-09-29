using ETZ.Application;
using ETZ.Application.DTOs.Topic;
using ETZ.Domain.Entities;
using ETZ.Persistence.Services;
using Microsoft.AspNetCore.Mvc;

namespace ETZ.Api.Controllers;

[ApiController]
[Route("api/topics")]
public sealed class TopicsController : ControllerBase
{
    private readonly TopicService _topicService;
    private readonly ILogger<TopicsController> _logger;

    public TopicsController(TopicService topicService, ILogger<TopicsController> logger)
    {
        _topicService = topicService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTopics([FromQuery] string languageCode = "tr")
    {
        if (!Enum.TryParse<LanguageCode>(languageCode, true, out var lang))
        {
            _logger.LogWarning("Invalid languageCode: {LanguageCode}", languageCode);
            return BadRequest($"Invalid languageCode: {languageCode}");
        }
        var topics = await _topicService.GetTopicsByLanguage(lang);
        if (topics.Count == 0)
        {
            _logger.LogWarning("No topics found");
            return NotFound("No topics found");
        }
        return Ok(topics);
    }

    [HttpPost]
    public async Task<ActionResult<Response>> CreateTopics([FromBody] TopicCreateUpdateDto dto)
    {
        var result = await _topicService.CreateAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Response>> UpdateTopics(Guid id, [FromBody] TopicCreateUpdateDto dto)
    {
        var result = await _topicService.UpdateAsync(id, dto);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Response>> DeleteTopics(Guid id)
    {
        var result = await _topicService.DeleteAsync(id);
        if (!result.Success)
        {
            return NotFound(result.Message);
        }
        return Ok(result);
    }

}


