using System.ComponentModel.DataAnnotations;

namespace ETZ.Domain.Entities;

public enum LanguageCode
{
    [Display(Name = "Türkçe")]
    tr,
    [Display(Name = "English")]
    en
}