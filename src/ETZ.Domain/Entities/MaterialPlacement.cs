namespace ETZ.Domain.Entities;
using ETZ.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MaterialPlacement : FullAuditedEntity<Guid>
{
    public Guid MaterialId {get;set;}
    public Material Material {get;set;} = null!;
    public required string PlacementCode {get;set;} 
   
}

public class MaterialPlacementConfiguration : IEntityTypeConfiguration<MaterialPlacement>
{
    public void Configure(EntityTypeBuilder<MaterialPlacement> builder)
    {
        builder.ToTable("MaterialPlacements");
        builder.Property(e => e.PlacementCode).HasMaxLength(255).IsRequired();
        builder.HasOne(e => e.Material).WithMany(e => e.MaterialPlacements).HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Cascade);
    }
}