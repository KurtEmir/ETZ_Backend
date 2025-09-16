using ETZ.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETZ.Domain.Entities;

public class Material : FullAuditedEntity<Guid>
{
    public string? MaterialName {get; set;}
    public required string MaterialUrl {get; set;}
    public MaterialType MaterialType {get; set;}
    public int DisplayOrder {get; set;}
    public ICollection<MaterialContent> MaterialContents {get; set;} = new List<MaterialContent>();
    public ICollection<MaterialPlacement> MaterialPlacements {get; set;} = new List<MaterialPlacement>();
}

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Materials");
        builder.Property(e => e.MaterialName).HasMaxLength(255);
        builder.Property(e => e.MaterialUrl).HasMaxLength(2048).IsRequired();
        builder.Property(e => e.MaterialType).HasConversion<string>()
        .HasMaxLength(50)
        .IsRequired();
        builder.HasIndex(e => e.DisplayOrder);
        builder.HasMany(e => e.MaterialContents).WithOne(e => e.Material).HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.MaterialPlacements).WithOne(e => e.Material).HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Cascade);
    }
}