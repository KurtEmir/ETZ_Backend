namespace ETZ.Persistence.Context;
using ETZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
public class ETZDbContext : DbContext
{
    public ETZDbContext(DbContextOptions<ETZDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ETZ.Domain.DomainAssemblyMarker).Assembly);
    }
    public DbSet<Sponsor> Sponsors {get; set;}
    public DbSet<SponsorType> SponsorTypes {get; set;}
    public DbSet<SponsorTypeContent> SponsorTypeContents {get; set;}
    public DbSet<Speaker> Speakers {get; set;}
    public DbSet<SpeakerContent> SpeakerContents {get; set;}
    public DbSet<Topic> Topics {get; set;}
    public DbSet<Material> Materials {get; set;}
    public DbSet<MaterialContent> MaterialContents {get; set;}
    public DbSet<MaterialPlacement> MaterialPlacements {get; set;}
}