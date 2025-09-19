namespace ETZ.Application.DTOs.Sponsor;

public class SponsorCreateUpdateDto
{
    public string Name { get; set; } = null!;
    public string SponsorLogoUrl { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public int SponsorTypeId { get; set; }
}