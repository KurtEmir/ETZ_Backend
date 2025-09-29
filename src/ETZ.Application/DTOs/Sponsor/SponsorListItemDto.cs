namespace ETZ.Application.DTOs.Sponsor;

public sealed class SponsorListItemDto
{
    public string SponsorName { get; set; } = string.Empty;
    public string SponsorLogoUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}


