using ETZ.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using ETZ.Domain.Entities;
using ETZ.Application.DTOs.Sponsor;
using ETZ.Application;

namespace ETZ.Api.Controllers;

// [Authorize]
[ApiController]
[Route("api/sponsors")]

public class SponsorsController : ControllerBase
{
    private readonly SponsorService _sponsorService;
    private readonly ILogger<SponsorsController> _logger;
    public SponsorsController(SponsorService sponsorService, ILogger<SponsorsController> logger)
    {
        _sponsorService = sponsorService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SponsorInformationDto>>> GetAllSponsors([FromQuery] string languageCode = "tr")
    {
        if (!Enum.TryParse<LanguageCode>(languageCode, true, out var lang))
        {
            _logger.LogWarning("Invalid languageCode: {LanguageCode}", languageCode);
            return BadRequest($"Invalid languageCode: {languageCode}");
        }

        var sponsors = await _sponsorService.GetSponsorsByLanguage(lang);
        if (sponsors.Count == 0)
        {
            _logger.LogWarning("No sponsors found");
            return NotFound("No sponsors found");
        }
        return Ok(sponsors); 
    }

    [HttpPost]
    public async Task<ActionResult<Response>> CreateSponsor([FromBody] SponsorCreateUpdateDto sponsorCreateUpdateDto)
    {
        var result = await _sponsorService.CreateSponsorAsync(sponsorCreateUpdateDto);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }



    




} 