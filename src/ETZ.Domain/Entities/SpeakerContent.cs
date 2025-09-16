namespace ETZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ETZ.Domain.Entities.Auditing;

public class SpeakerContent : FullAuditedEntity<Guid>
{
    public string? Title {get; set;} 
    public Speaker Speaker {get; set;} = null!;
    public Guid SpeakerId {get; set;}
    public LanguageCode LanguageCode {get; set;}
    public string? Description {get; set;}
}

public class SpeakerContentConfiguration : IEntityTypeConfiguration<SpeakerContent>
{
    public void Configure(EntityTypeBuilder<SpeakerContent> builder)
    {
        builder.ToTable("SpeakerContents");
        builder.Property(e => e.Title).HasMaxLength(255);
        builder.HasOne(e => e.Speaker).WithMany(e => e.SpeakerContent).HasForeignKey(e => e.SpeakerId).OnDelete(DeleteBehavior.Cascade); 
        builder.Property(e => e.LanguageCode).HasConversion<string>()
        .HasMaxLength(2)
        .IsRequired();
        builder.Property(e => e.Description).HasMaxLength(1500);
    }
}