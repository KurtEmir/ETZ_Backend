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
    private readonly TopicService _service;
    private readonly ILogger<TopicsController> _logger;

    public TopicsController(TopicService service, ILogger<TopicsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string languageCode = "tr")
    {
        if (!Enum.TryParse<LanguageCode>(languageCode, true, out var lang))
        {
            _logger.LogWarning("Invalid languageCode: {LanguageCode}", languageCode);
            return BadRequest($"Invalid languageCode: {languageCode}");
        }
        var topics = await _service.GetTopicsByLanguage(lang);
        if (topics.Count == 0)
        {
            _logger.LogWarning("No topics found");
            return NotFound("No topics found");
        }
        return Ok(topics);
    }

    [HttpPost]
    public async Task<ActionResult<Response>> Create([FromBody] TopicCreateUpdateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }

   
}


