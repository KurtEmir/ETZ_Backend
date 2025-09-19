namespace ETZ.Domain.Entities;

using ETZ.Domain.Entities.Auditing; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SponsorTypeContent : FullAuditedEntity<int>
{
    public string Name {get; set;} = null!;
    public LanguageCode LanguageCode {get; set;}
    public int SponsorTypeId {get; set;}
    public SponsorType SponsorType {get; set;} = null!;
}

public class SponsorTypeContentConfiguration : IEntityTypeConfiguration<SponsorTypeContent>
{
    public void Configure(EntityTypeBuilder<SponsorTypeContent> builder)
    {
        builder.ToTable("SponsorTypeContents");

        builder.Property(x => x.Name)
         .HasMaxLength(150)
         .IsRequired();

        builder.Property(x => x.LanguageCode)
         .HasConversion<string>()
         .HasMaxLength(2)
         .IsRequired();

         builder.HasKey(x => x.Id);
    }
}