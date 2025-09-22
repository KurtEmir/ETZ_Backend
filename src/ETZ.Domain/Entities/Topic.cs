namespace ETZ.Domain.Entities;

using ETZ.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Topic : FullAuditedEntity<Guid>
{   
    public int DisplayOrder { get; set; }
    public string TopicName { get; set; } = null!;
    public LanguageCode LanguageCode { get; set; }
}

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("Topics");
        builder.Property(e => e.TopicName).HasMaxLength(255).IsRequired();
        builder.Property(e => e.LanguageCode).HasConversion<string>()
        .HasMaxLength(2)
        .IsRequired();
    }
}