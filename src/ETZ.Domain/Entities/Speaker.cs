namespace ETZ.Domain.Entities;

using ETZ.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Speaker : FullAuditedEntity<Guid>
{
    public string Name {get; set;} = null!;
    public string Surname {get; set;} = null!;
    public string SpeakerPhotoUrl {get; set;} = null!;
    public int DisplayOrder {get; set;}
    public ICollection<SpeakerContent> SpeakerContent {get; set;} = new List<SpeakerContent>();
}

public class SpeakerConfiguration : IEntityTypeConfiguration<Speaker>
{
    public void Configure(EntityTypeBuilder<Speaker> builder)
    {
        builder.ToTable("Speakers");
        builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
        builder.Property(e => e.Surname).HasMaxLength(255).IsRequired();
        builder.Property(e => e.SpeakerPhotoUrl).HasMaxLength(2048).IsRequired();
        builder.HasIndex(e => e.DisplayOrder);
    }
}