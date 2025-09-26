namespace ETZ.Application.DTOs.Sponsor;

public class SponsorInformationDto
{
    public Guid Id { get; set; }
    public string SponsorName { get; set; } = null!;
    public string SponsorLogoUrl { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public string SponsorTypeName { get; set; } = null!;
}