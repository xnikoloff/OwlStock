using System.ComponentModel.DataAnnotations;

namespace OwlStock.Domain.Enumerations
{
    public enum PhotoSize
    {
        [Display(Name = "Пълен размер")]
        OriginalSize = 1,

        [Display(Name = "Малка")]
        Small = 2,

        [Display(Name = "Средна")]
        Medium = 3,

        [Display(Name = "Голяма")]
        Large = 4
    }
}
