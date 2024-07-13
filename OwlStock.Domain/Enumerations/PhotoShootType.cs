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

        [Display(Name = "Сватба+")]
        WeddingPlus = 4,

        [Display(Name = "Абитуриентски бал")]
        Prom = 5,

        [Display(Name = "Детска фотосесия")]
        Kids = 6,

        [Display(Name = "Имот/сграда")]
        Property = 7,

        //[Display(Name = "Събитие")]
        //Event = 8,

        [Display(Name = "Автомобил")]
        Automotive = 9,

        [Display(Name = "Друг")]
        Other = 10
    }
}
