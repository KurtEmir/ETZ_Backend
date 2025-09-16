namespace ETZ.Domain.Entities;

using ETZ.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MaterialContent : FullAuditedEntity<Guid>
{
    public Material Material {get;set;} = null!;
    public Guid MaterialId {get;set;}
    public LanguageCode LanguageCode {get;set;}
    public string? Title {get;set;}
    public string? Description {get;set;}
}

public class MaterialContentConfiguration : IEntityTypeConfiguration<MaterialContent>
{
    public void Configure(EntityTypeBuilder<MaterialContent> builder)
    {
        builder.ToTable("MaterialContents");
        builder.Property(e => e.Title).HasMaxLength(255);
        builder.HasOne(e => e.Material).WithMany(e => e.MaterialContents).HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Cascade);
        builder.Property(e => e.LanguageCode).HasConversion<string>()
        .HasMaxLength(2)
        .IsRequired();
        builder.Property(e => e.Description).HasColumnType("text");
        builder.HasIndex(e => new { e.MaterialId, e.LanguageCode }).IsUnique();
    }
}