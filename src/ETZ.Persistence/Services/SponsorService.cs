using ETZ.Persistence.Context;
using Microsoft.Extensions.Logging;
using ETZ.Domain.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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
}