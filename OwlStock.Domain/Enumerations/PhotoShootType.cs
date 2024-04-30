using System.ComponentModel.DataAnnotations;

namespace OwlStock.Domain.Enumerations
{
    public enum PhotoShootType
    {
        [Display(Name = "Лична фотосесия")]
        Personal = 1,

        [Display(Name = "Сватба")]
        Wedding = 2,

        [Display(Name = "Лично празненство")]
        Party = 3,

        [Display(Name = "Детска фотосесия")]
        Kids = 4,

        [Display(Name = "Имот/сграда")]
        Property = 5,

        [Display(Name = "Събитие")]
        Event = 6,

        [Display(Name = "Автомобил")]
        Automotive = 7,

        [Display(Name = "Друг")]
        Other = 8
    }
}
