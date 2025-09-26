using ETZ.Persistence.Context;
using Microsoft.Extensions.Logging;
using ETZ.Domain.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ETZ.Application.DTOs.Sponsor;
using ETZ.Application;

namespace ETZ.Persistence.Services;

public class SponsorService
{
    //ICurrentUserGelecek
    private readonly ETZDbContext _context;
    private readonly ILogger<SponsorService> _logger;

    public SponsorService(ETZDbContext context, ILogger<SponsorService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // public async Task<List<Sponsor>> GetAllSponsors(Expression<Func<Sponsor, bool>> predicate)
    // {
    //     var sponsors = await _context.Sponsors.AsTracking().Where(predicate).ToListAsync();
    //     return sponsors;
    // }
    public async Task<Response> CreateSponsorAsync(SponsorCreateUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.SponsorLogoUrl))
            return Response.Fail("Name and SponsorLogoUrl are required");
        if (dto.Translations is null || dto.Translations.Count == 0)
            return Response.Fail("At least one translation is required");

        var sponsor = new Sponsor
        {
            Id = Guid.NewGuid(),
            Name = dto.Name.Trim(),
            SponsorLogoUrl = dto.SponsorLogoUrl.Trim(),
            DisplayOrder = dto.DisplayOrder,
            CreationTime = DateTimeOffset.UtcNow,
            CreatorUserId = Constants.SystemGodUserId
        };
        await _context.Sponsors.AddAsync(sponsor);

        foreach (var tr in dto.Translations.GroupBy(t => t.LanguageCode).Select(g => g.First()))
        {
            await _context.SponsorTypes.AddAsync(new SponsorType
            {
                SponsorId = sponsor.Id,
                LanguageCode = tr.LanguageCode,
                Type = tr.Type.Trim(),
                CreationTime = DateTimeOffset.UtcNow,
                CreatorUserId = Constants.SystemGodUserId
            });
        }

        await _context.SaveChangesAsync();
        return Response.Ok("Sponsor created successfully");
    }

    public async Task<List<SponsorInformationDto>> GetSponsorsByLanguage(LanguageCode lang)
    {
        return await _context.Sponsors
            .AsNoTracking()
            .Where(s => !s.IsDeleted && s.SponsorTypes.Any(c => c.LanguageCode == lang && !c.IsDeleted))
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new SponsorInformationDto
            {
                Id = s.Id,
                SponsorName = s.Name,
                SponsorLogoUrl = s.SponsorLogoUrl,
                DisplayOrder = s.DisplayOrder,
                SponsorTypeName = s.SponsorTypes
                    .Where(c => c.LanguageCode == lang && !c.IsDeleted)
                    .Select(c => c.Type)
                    .FirstOrDefault() ?? string.Empty
            })
            .ToListAsync();
    }

    public async Task<Response> DeleteSponsorAsync(Guid id)
    {
        var affected = await _context.Sponsors
            .Where(s => s.Id == id && !s.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.IsDeleted, true)
                .SetProperty(s => s.DeletionTime, DateTimeOffset.UtcNow)
                .SetProperty(s => s.DeleterUserId, Constants.SystemGodUserId));

        if (affected == 0)
            return Response.Fail("Sponsor not found");

        // Soft delete child localized types as well
        await _context.SponsorTypes
            .Where(t => t.SponsorId == id && !t.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(t => t.IsDeleted, true)
                .SetProperty(t => t.DeletionTime, DateTimeOffset.UtcNow)
                .SetProperty(t => t.DeleterUserId, Constants.SystemGodUserId));

        return Response.Ok("Sponsor deleted");
    }
    
}