using System.ComponentModel.DataAnnotations;

namespace OwlStock.Domain.Enumerations
{
    public enum PhotoShootType
    {
        [Display(Name = "Лична фотосесия")]
        Personal = 1,

        [Display(Name = "Фотосесия за бременни")]
        Pregnant = 2,

        [Display(Name = "Сватба")]
        Wedding = 3,

        [Display(Name = "Абитуриентски бал")]
        Prom = 4,

        [Display(Name = "Детска фотосесия")]
        Kids = 5,

        [Display(Name = "Имот/сграда")]
        Property = 6,

        [Display(Name = "Събитие")]
        Event = 7,

        [Display(Name = "Автомобил")]
        Automotive = 8,

        [Display(Name = "Друг")]
        Other = 9
    }
}
