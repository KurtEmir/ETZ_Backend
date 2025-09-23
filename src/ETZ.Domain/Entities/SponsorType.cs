namespace ETZ.Domain.Entities;

using ETZ.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SponsorType : FullAuditedEntity<int>
{ 
    public Guid SponsorId { get; set; }
    public Sponsor Sponsor { get; set; } = null!;
    public LanguageCode LanguageCode { get; set; }
    public string Type { get; set; } = string.Empty;
}

public class SponsorTypeConfiguration : IEntityTypeConfiguration<SponsorType>
{
    public void Configure(EntityTypeBuilder<SponsorType> builder)
    {
        builder.ToTable("SponsorTypes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.LanguageCode)
            .HasConversion<string>()
            .HasMaxLength(2)
            .IsRequired();

        builder.HasOne(x => x.Sponsor)
            .WithMany(s => s.SponsorTypes)
            .HasForeignKey(x => x.SponsorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.SponsorId, x.LanguageCode}).IsUnique();
    }
}