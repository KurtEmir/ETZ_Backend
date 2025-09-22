using ETZ.Application;
using ETZ.Application.DTOs.Speaker;
using ETZ.Domain.Entities;
using ETZ.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ETZ.Persistence.Services;

public sealed class SpeakerService
{
    private readonly ETZDbContext _context;
    private readonly ILogger<SpeakerService> _logger;

    public SpeakerService(ETZDbContext context, ILogger<SpeakerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Speaker>> GetAllAsync()
        => await _context.Speakers.AsNoTracking().Where(x => !x.IsDeleted).OrderBy(x => x.DisplayOrder).ToListAsync();

    public async Task<Speaker?> GetByIdAsync(Guid id)
        => await _context.Speakers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

    public async Task<Response> CreateAsync(SpeakerCreateUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Surname))
            return Response.Fail("Name and Surname are required");

        var speaker = new Speaker
        {
            Id = Guid.NewGuid(),
            Name = dto.Name.Trim(),
            Surname = dto.Surname.Trim(),
            SpeakerPhotoUrl = dto.SpeakerPhotoUrl,
            DisplayOrder = dto.DisplayOrder,
            CreationTime = DateTimeOffset.UtcNow,
            CreatorUserId = Constants.SystemGodUserId
        };

        await _context.Speakers.AddAsync(speaker);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Speaker created Id={Id}", speaker.Id);
        return Response.Ok("Speaker created successfully");
    }

    public async Task<Response> UpdateAsync(Guid id, SpeakerCreateUpdateDto dto)
    {
        var entity = await _context.Speakers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) return Response.Fail("Speaker not found");

        entity.Name = dto.Name.Trim();
        entity.Surname = dto.Surname.Trim();
        entity.SpeakerPhotoUrl = dto.SpeakerPhotoUrl;
        entity.DisplayOrder = dto.DisplayOrder;
        entity.LastModifierUserId = Constants.SystemGodUserId;
        entity.LastModificationTime = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Speaker updated Id={Id}", id);
        return Response.Ok("Speaker updated successfully");
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        var entity = await _context.Speakers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) return Response.Fail("Speaker not found");

        entity.IsDeleted = true;
        entity.DeleterUserId = Constants.SystemGodUserId;
        entity.DeletionTime = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Speaker deleted Id={Id}", id);
        return Response.Ok("Speaker deleted successfully");
    }

    public async Task<List<SpeakerInformationDto>> GetSpeakersByLanguage(LanguageCode lang)
    {
        return await _context.Speakers
        .AsNoTracking()
        .Where(s => !s.IsDeleted)
        .OrderBy(s => s.DisplayOrder)
        .Select(s => new SpeakerInformationDto
        {
            SpeakerName = s.Name,
            SpeakerSurname = s.Surname,
            SpeakerPhotoUrl = s.SpeakerPhotoUrl,
            SpeakerTitle = s.SpeakerContent
                .Where(c => c.LanguageCode == lang && !c.IsDeleted)
                .Select(c => c.Title)
                .FirstOrDefault() ?? string.Empty,
            DisplayOrder = s.DisplayOrder,
                            
        })
        .ToListAsync();
    }

    public async Task<Response> CreateSpeakerContentAsync(SpeakerContentDto dto)
    {
        if (dto == null)
            return Response.Fail("SpeakerContentDto is required");

        var speakerContent = new SpeakerContent
        {
            Id = Guid.NewGuid(),
            SpeakerId = dto.SpeakerId,
            Title = dto.Title,
            LanguageCode = dto.LanguageCode,
            CreationTime = DateTimeOffset.UtcNow,
            CreatorUserId = Constants.SystemGodUserId,
        };
        await _context.SpeakerContents.AddAsync(speakerContent);
        await _context.SaveChangesAsync();
        _logger.LogInformation("SpeakerContent created Id={Id}", speakerContent.Id);
        return Response.Ok("SpeakerContent created successfully");
    }
}


