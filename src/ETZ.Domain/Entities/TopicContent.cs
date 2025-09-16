namespace ETZ.Domain.Entities;

using ETZ.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TopicContent : FullAuditedEntity<Guid>
{
    public Topic Topic { get; set; } = null!;
    public Guid TopicId { get; set; }
    public LanguageCode LanguageCode { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
}

public class TopicContentConfiguration : IEntityTypeConfiguration<TopicContent>
{
    public void Configure(EntityTypeBuilder<TopicContent> builder)
    {
        builder.ToTable("TopicContents");
        builder.Property(e => e.Title).HasMaxLength(255).IsRequired();
        builder.HasOne(e => e.Topic).WithMany(e => e.TopicContents).HasForeignKey(e => e.TopicId);
        builder.Property(e => e.LanguageCode).HasConversion<string>()
        .HasMaxLength(2)
        .IsRequired();;
    }
}