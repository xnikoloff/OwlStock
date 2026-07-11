using System.ComponentModel.DataAnnotations;

namespace OwlStock.Domain.Enumerations
{
    public enum GiftCardStatus
    {
        [Display(Name = "Активен")]
        New = 1,

        [Display(Name = "Валидиран")]
        Validated = 2,

        [Display(Name = "Отказан")]
        Declined = 3,

        [Display(Name = "Използван")]
        Used = 4,
    }
}
