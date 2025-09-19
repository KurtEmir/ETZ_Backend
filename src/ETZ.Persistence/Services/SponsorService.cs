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

    public async Task<List<Sponsor>> GetAllSponsors(Expression<Func<Sponsor, bool>> predicate)
    {
        var sponsors = await _context.Sponsors.AsTracking().Where(predicate).ToListAsync();
        return sponsors;
    }

    public async Task<Sponsor?> GetSponsorAsync(Expression<Func<Sponsor, bool>> predicate)
    {
        var sponsor = await _context.Sponsors.AsTracking().Where(predicate).FirstOrDefaultAsync();
        return sponsor;
    }

    public async Task<Sponsor?> GetSponsorByIdAsync(Guid id)
    {
        return await GetSponsorAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Response> CreateSponsorAsync(SponsorCreateUpdateDto sponsorCreateDto)
    {                
        _logger.LogInformation("Creating sponsor Name={Name}, TypeId={TypeId}", sponsorCreateDto.Name, sponsorCreateDto.SponsorTypeId);
        var sponsor = new Sponsor
        {
            Id = Guid.NewGuid(),
            Name = sponsorCreateDto.Name,
            SponsorLogoUrl = sponsorCreateDto.SponsorLogoUrl,
            DisplayOrder = sponsorCreateDto.DisplayOrder,
            SponsorTypeId = sponsorCreateDto.SponsorTypeId,
            CreationTime = DateTimeOffset.UtcNow,
            CreatorUserId = Constants.SystemGodUserId,
            IsDeleted = false,
            DeletionTime = null,
            DeleterUserId = null,
            LastModificationTime = null,
            LastModifierUserId = null
        };
        var sponsorType = await _context.SponsorTypes.FindAsync(sponsorCreateDto.SponsorTypeId);
        if (sponsorType == null)
        {
            _logger.LogError("Sponsor type not found");
            return Response.Fail("Sponsor type not found");
        }
        sponsor.SponsorType = sponsorType;
        await _context.Sponsors.AddAsync(sponsor);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Sponsor created Id={Id}", sponsor.Id);
        return Response.Ok("Sponsor created successfully");
    }

    public async Task<Response> UpdateSponsorAsync(Guid Id, SponsorCreateUpdateDto sponsorUpdateDto)
    {
        _logger.LogInformation("Updating sponsor Id={Id}", Id);
        if (sponsorUpdateDto == null)
        {
            _logger.LogWarning("UpdateSponsorAsync received null DTO");
            return Response.Fail("Update model not found");
        }
        var sponsor = await GetSponsorByIdAsync(Id);
        if (sponsor == null)
        {
            _logger.LogError("Sponsor not found");
            return Response.Fail("Sponsor not found");
        }
        sponsor.Name = sponsorUpdateDto.Name;
        sponsor.SponsorLogoUrl = sponsorUpdateDto.SponsorLogoUrl;
        sponsor.DisplayOrder = sponsorUpdateDto.DisplayOrder;
        sponsor.SponsorTypeId = sponsorUpdateDto.SponsorTypeId;
        sponsor.LastModificationTime = DateTimeOffset.UtcNow;
        sponsor.LastModifierUserId = Constants.SystemGodUserId;
        bool sponsorTypeExists = await _context.SponsorTypes.AnyAsync(x => x.Id == sponsorUpdateDto.SponsorTypeId);
        if (!sponsorTypeExists)
        {
            _logger.LogWarning("Sponsor type not found for update TypeId={TypeId}", sponsorUpdateDto.SponsorTypeId);
            return Response.Fail("Sponsor type not found");
        }
        await _context.SaveChangesAsync();
        _logger.LogInformation("Sponsor updated Id={Id}", Id);
        return Response.Ok("Sponsor updated successfully");
    }

    public async Task<Response> DeleteSponsorAsync(Guid id)
    {
        _logger.LogInformation("Deleting sponsor Id={Id}", id);
        var sponsor = await GetSponsorByIdAsync(id);
        if (sponsor == null)
        {
            _logger.LogError("Sponsor not found");
            return Response.Fail("Sponsor not found");
        }
        sponsor.IsDeleted = true;
        sponsor.DeleterUserId = Constants.SystemGodUserId;
        sponsor.DeletionTime = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Sponsor deleted Id={Id}", id);
        return Response.Ok("Sponsor deleted successfully");
    }

    public async Task<List<SponsorType>> GetAllSponsorTypes()
    {
        return await _context.SponsorTypes.AsTracking().Where(x => !x.IsDeleted).ToListAsync();
    }

    public async Task<SponsorType?> GetSponsorTypeById(int id)
    {
        return await _context.SponsorTypes.AsTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Response> CreateSponsorType(CreateSponsorTypeDto sponsorTypeDto)
    {
        if (string.IsNullOrWhiteSpace(sponsorTypeDto.SponsorCode)) 
        {
            _logger.LogWarning("CreateSponsorType validation failed: empty code");
            return Response.Fail("Code required");
        }
        var code = sponsorTypeDto.SponsorCode.Trim();
        var exists = await _context.SponsorTypes.AsNoTracking().AnyAsync(x => x.SponsorCode == code);
        if (exists)
        {
            _logger.LogWarning("CreateSponsorType duplicate code={Code}", code);
            return Response.Fail("Code already exists");
        }
        var sponsorType = new SponsorType
        {
            SponsorCode = code,
            CreationTime = DateTimeOffset.UtcNow,
            CreatorUserId = Constants.SystemGodUserId,
            IsDeleted = false,
            DeletionTime = null,
            DeleterUserId = null,
            LastModificationTime = null,
            LastModifierUserId = null
        };

        await _context.SponsorTypes.AddAsync(sponsorType);
        await _context.SaveChangesAsync();
        _logger.LogInformation("SponsorType created Id={Id} Code={Code}", sponsorType.Id, sponsorType.SponsorCode);
        return Response.Ok("Sponsor type created successfully");
    }

    public async Task<Response> UpdateSponsorType(int id, SponsorType sponsorType)
    {
        _logger.LogInformation("Updating SponsorType Id={Id}", id);
        var existing = await _context.SponsorTypes.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (existing is null)
        {
            _logger.LogError("SponsorType not found Id={Id}", id);
            return Response.Fail("Sponsor type not found");
        }

        var code = (sponsorType.SponsorCode ?? "").Trim();
        if (string.IsNullOrWhiteSpace(code))
        {
            _logger.LogWarning("UpdateSponsorType validation failed: empty code Id={Id}", id);
            return Response.Fail("Code required");
        }
        var exists = await _context.SponsorTypes.AnyAsync(x => x.SponsorCode == code && x.Id != id);
        if (exists)
        {
            _logger.LogWarning("UpdateSponsorType duplicate code={Code} Id={Id}", code, id);
            return Response.Fail("Code already exists");
        }

        existing.SponsorCode = code;
        existing.LastModifierUserId = Constants.SystemGodUserId;
        existing.LastModificationTime = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("SponsorType updated Id={Id}", id);
        return Response.Ok("Sponsor type updated successfully");
    }

    public async Task<Response> DeleteSponsorType(int id)
    {
        _logger.LogInformation("Deleting SponsorType Id={Id}", id);
        var existingSponsorType = await GetSponsorTypeById(id);
        if (existingSponsorType == null)
        {
            _logger.LogError("Sponsor type not found");
            return Response.Fail("Sponsor type not found");
        }
        existingSponsorType.IsDeleted = true;
        existingSponsorType.DeleterUserId = Constants.SystemGodUserId;
        existingSponsorType.DeletionTime = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        _logger.LogInformation("SponsorType deleted Id={Id}", id);
        return Response.Ok("Sponsor type deleted successfully");
    }

    public async Task<List<SponsorTypeContent>> GetAllSponsorTypeContents()
    {
        return await _context.SponsorTypeContents.AsTracking().Where(x => !x.IsDeleted).ToListAsync();
    }

    public async Task<SponsorTypeContent?> GetSponsorTypeContentById(int id)
    {
        return await _context.SponsorTypeContents.AsTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Response> CreateSponsorTypeContent(CreateSponsorTypeContentDto sponsorTypeContentDto)
    {
        if (string.IsNullOrWhiteSpace(sponsorTypeContentDto.TypeName))
        {
            _logger.LogWarning("CreateSponsorTypeContent validation failed: empty name");
            return Response.Fail("Name required");
        }
        var name = sponsorTypeContentDto.TypeName.Trim();
        var exists = await _context.SponsorTypeContents.AnyAsync(x => x.Name == name && x.SponsorTypeId == sponsorTypeContentDto.SponsorTypeId && x.LanguageCode == sponsorTypeContentDto.LanguageCode);
        if (exists)
        {
            _logger.LogWarning("CreateSponsorTypeContent duplicate name={Name} SponsorTypeId={SponsorTypeId} LanguageCode={LanguageCode}", name, sponsorTypeContentDto.SponsorTypeId, sponsorTypeContentDto.LanguageCode);
            return Response.Fail("Oluşturmak istediğiniz sponsor type content zaten mevcuttur.");
        }
        sponsorTypeContentDto.TypeName = name;
        var sponsorTypeContent = new SponsorTypeContent
        {
            Name = name,
            SponsorTypeId = sponsorTypeContentDto.SponsorTypeId,
            LanguageCode = sponsorTypeContentDto.LanguageCode,
            CreationTime = DateTimeOffset.UtcNow,
            CreatorUserId = Constants.SystemGodUserId,
            IsDeleted = false,
            DeletionTime = null,
            DeleterUserId = null,
            LastModificationTime = null,
            LastModifierUserId = null
        };
        await _context.SponsorTypeContents.AddAsync(sponsorTypeContent);
        await _context.SaveChangesAsync();
        _logger.LogInformation("SponsorTypeContent created Id={Id} Name={Name}", sponsorTypeContent.Id, sponsorTypeContent.Name);
        return Response.Ok("Sponsor type content created successfully");
    }

    public async Task<Response> UpdateSponsorTypeContent(int id, SponsorTypeContent sponsorTypeContent)
    {
        _logger.LogInformation("Updating SponsorTypeContent Id={Id}", id);
        var existing = await GetSponsorTypeContentById(id);
        if (existing is null)
        {
            _logger.LogError("SponsorTypeContent not found Id={Id}", id);
            return Response.Fail("Sponsor type content not found");
        }
        var name = sponsorTypeContent.Name.Trim();
        var exists = await _context.SponsorTypeContents.AnyAsync(x => x.Name == name && x.Id != id && x.SponsorTypeId == sponsorTypeContent.SponsorTypeId && x.LanguageCode == sponsorTypeContent.LanguageCode);
        if (exists)
        {
            _logger.LogWarning("UpdateSponsorTypeContent duplicate name={Name} Id={Id} SponsorTypeId={SponsorTypeId} LanguageCode={LanguageCode}", name, id, sponsorTypeContent.SponsorTypeId, sponsorTypeContent.LanguageCode);
            return Response.Fail("Güncellemek istediğiniz sponsor type content zaten mevcuttur.");
        }
        existing.Name = name;
        existing.LastModifierUserId = Constants.SystemGodUserId;
        existing.LastModificationTime = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        _logger.LogInformation("SponsorTypeContent updated Id={Id}", id);
        return Response.Ok("Sponsor type content updated successfully");
    }


    public async Task<List<SponsorInformationDto>> GetSponsorsByLanguage(LanguageCode lang)
    {
        return await _context.Sponsors
        .AsNoTracking()
        .Where(s => !s.IsDeleted)
        .OrderBy(s => s.DisplayOrder)
        .Select(s => new SponsorInformationDto
        {
            SponsorName = s.Name,
            SponsorLogoUrl = s.SponsorLogoUrl,
            DisplayOrder = s.DisplayOrder,
            SponsorTypeName = s.SponsorType.SponsorTypeContent
                .Where(c => c.LanguageCode == lang && !c.IsDeleted)
                .Select(c => c.Name)
                .FirstOrDefault() ?? s.SponsorType.SponsorCode
        })
        .ToListAsync();
    }
}