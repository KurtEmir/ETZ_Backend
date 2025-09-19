namespace ETZ.Domain.Entities;

using ETZ.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Sponsor : FullAuditedEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string SponsorLogoUrl { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public SponsorType SponsorType { get; set; } = null!;
    public int SponsorTypeId {get; set;}
}

public class SponsorConfiguration : IEntityTypeConfiguration<Sponsor>
{
    public void Configure(EntityTypeBuilder<Sponsor> builder)
    {
        builder.ToTable("Sponsors");
        builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
        builder.Property(e => e.SponsorLogoUrl).HasMaxLength(2048).IsRequired();
        builder.HasIndex(e => e.DisplayOrder);
        builder.HasOne(s => s.SponsorType)
        .WithMany(t => t.Sponsors) 
        .HasForeignKey(s => s.SponsorTypeId)
        .OnDelete(DeleteBehavior.Restrict); //TODO araştır bu restrict'i
    }   
}

