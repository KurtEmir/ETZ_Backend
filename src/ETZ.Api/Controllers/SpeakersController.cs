using ETZ.Application;
using ETZ.Application.DTOs.Speaker;
using ETZ.Domain.Entities;
using ETZ.Persistence.Services;
using Microsoft.AspNetCore.Mvc;

namespace ETZ.Api.Controllers;
[ApiController]
[Route("api/speakers")]
public sealed class SpeakersController : ControllerBase
{
    private readonly SpeakerService _speakerService;
    private readonly ILogger<SpeakersController> _logger;

    public SpeakersController(SpeakerService speakerService, ILogger<SpeakersController> logger)
    {
        _speakerService = speakerService;
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
        var speakers = await _speakerService.GetSpeakersByLanguage(lang);
        if (speakers.Count == 0)
        {
            _logger.LogWarning("No speakers found");
            return NotFound("No speakers found");
        }
        return Ok(speakers);
    }

    
    [HttpPost]
    public async Task<ActionResult<Response>> Create([FromBody] SpeakerCreateUpdateDto dto)
    {
        var result = await _speakerService.CreateAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }

    [HttpPost("speaker-content")]
    public async Task<ActionResult<Response>> CreateSpeakerContent([FromBody] SpeakerContentDto dto)
    {
        var result = await _speakerService.CreateSpeakerContentAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }

}


