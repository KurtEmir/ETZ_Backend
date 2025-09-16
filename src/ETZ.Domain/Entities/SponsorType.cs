namespace ETZ.Domain.Entities;

using ETZ.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SponsorType : FullAuditedEntity<int>
{
    public string SponsorCode {get; set;} = null!; // sponsor türünün ismi olcak kod gibi çağrışım yapsın diye yani
    public ICollection<SponsorTypeContent> SponsorTypeContent {get; set;} = new List<SponsorTypeContent>();
    public ICollection<Sponsor> Sponsors {get; set;} = new List<Sponsor>();
}

public class SponsorTypeConfiguration : IEntityTypeConfiguration<SponsorType>
{
    public void Configure(EntityTypeBuilder<SponsorType> builder)
    {
        builder.ToTable("SponsorTypes");
        
        builder.HasIndex(e => e.SponsorCode).IsUnique(true);
        builder.Property(e => e.SponsorCode)
        .HasMaxLength(50)
        .IsRequired(true);

        builder.HasMany(e => e.SponsorTypeContent)
        .WithOne(e => e.SponsorType)
        .HasForeignKey(e => e.SponsorTypeId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}