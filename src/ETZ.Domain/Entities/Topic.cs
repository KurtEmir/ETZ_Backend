namespace ETZ.Domain.Entities;

using ETZ.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Topic : FullAuditedEntity<Guid>
{
    public int DisplayOrder { get; set; }
    public ICollection<TopicContent> TopicContents { get; set; } = new List<TopicContent>();
}

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("Topics");
        builder.HasMany(e => e.TopicContents)
        .WithOne(e => e.Topic)
        .HasForeignKey(e => e.TopicId)
        .OnDelete(DeleteBehavior.Cascade); //TODO araştır bu cascade'i
    }
}