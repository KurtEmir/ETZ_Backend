using ETZ.Domain.Entities;
namespace ETZ.Application.DTOs.Sponsor;

public class CreateSponsorTypeContentDto
{
    public string TypeName {get;set;} = null!;
    public LanguageCode LanguageCode {get;set;}
    public int SponsorTypeId {get;set;}
}