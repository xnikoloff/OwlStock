using System.ComponentModel.DataAnnotations;

namespace OwlStock.Domain.Enumerations
{
    public enum PhotoDeliveryMethod
    {
        [Display(Name = "Имейл")]
        Email = 1,

        [Display(Name = "USB памет")]
        USB = 2,

        [Display(Name = "CD диск")]
        CD = 3
    }
}
