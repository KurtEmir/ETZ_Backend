using ETZ.Persistence.Context;
using Microsoft.Extensions.Logging;
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
}