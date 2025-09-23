using Microsoft.Extensions.DependencyInjection;
using ETZ.Persistence.Services;


namespace ETZ.Persistence.ServiceRegistiration;

public static class ServiceRegistiration
{
    public static void AddPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<SponsorService>();
        services.AddScoped<SpeakerService>();
        services.AddScoped<TopicService>();
        services.AddScoped<MaterialService>();
    }
}