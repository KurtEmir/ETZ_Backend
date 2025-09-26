using ETZ.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using ETZ.Domain.Entities;
using ETZ.Application;
using ETZ.Application.DTOs.Material;

namespace ETZ.Api.Controllers;
[ApiController]
[Route("api/materials")]
public class MaterialController : ControllerBase
{
    private readonly MaterialService _materialService;
    private readonly ILogger<MaterialController> _logger;

    public MaterialController(MaterialService materialService, ILogger<MaterialController> logger)
    {
        _materialService = materialService;
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
        var materials = await _materialService.GetMaterialsByLanguage(lang);
        if (materials.Count == 0)
        {
            _logger.LogWarning("No materials found");
            return NotFound("No materials found");
        }
        return Ok(materials);
    }
    
    [HttpPost]
    public async Task<ActionResult<Response>> Create([FromBody] MaterialCreateUpdateDto dto)
    {
        var result = await _materialService.CreateAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Response>> Delete(Guid id)
    {
        var result = await _materialService.DeleteAsync(id);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }
    
}