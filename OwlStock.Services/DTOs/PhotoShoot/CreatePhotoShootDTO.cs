using OwlStock.Domain.Common;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.Common.HelperClasses;
using System.ComponentModel.DataAnnotations;

namespace OwlStock.Services.DTOs.PhotoShoot
{
    public class CreatePhotoShootDTO
    {
        [Required]
        [Display(Name = "Име")]
        [MaxLength(ModelConstraints.PersonNameMaxLength)]
        public string? PersonFirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        [MaxLength(ModelConstraints.PersonNameMaxLength)]
        public string? PersonLastName { get; set; }

        [MaxLength(ModelConstraints.PersonEmailMaxLength)]
        public string? PersonEmail { get; set; }

        [Required]
        [Display(Name = "Телефон")]
        [MaxLength(ModelConstraints.PersonPhoneMaxLenth)]
        public string? PersonPhone { get; set; }

        [Required]
        [Display(Name = "Дата")]
        public DateTime ReservationDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Час")]
        public TimeOnly ReservationTime { get; set; }

        [Required]
        [Display(Name = "Тип на фотосесията")]
        public PhotoShootType PhotoShootType { get; set; }

        [Display(Name = "Описание")]
        [MaxLength(ModelConstraints.PhotoShootTypeDescriptionMaxLength)]
        public string? PhotoShootTypeDescription { get; set; }

        //not used for now
        //[Required]
        //public string? SettlementName { get; set; }

        [Display(Name = "Място на фотосесията")]
        [MaxLength(ModelConstraints.UserPlace)]
        [Required(ErrorMessage = "Място на фотосесията е задължително")]
        public string? UserPlace { get; set; }

        [Display(Name = "Нека ние изберем място за Вас")]
        public bool IsDecidedByUs { get; set; }

        public bool IsPlaceSelected { get; set; }

        [Display(Name = "Линк към Google Maps")]
        public string? GoogleMapsLink { get; set; }

        public decimal FuelPrice { get; set; }

        public decimal Price { get; set; }

        [Display(Name = "Не желая да получа снимките си чрез сайта на DreamPix")]
        public bool DoNotUploadPhotos { get; set; }

        [Display(Name = "Начин на получаване на снимки")]
        public PhotoDeliveryMethod? PhotoDeliveryMethod { get; set; }

        [Display(Name = "Адрес или офис на Econt")]
        public string? PhotoDeliveryAddress { get; set; }

        public Dictionary<DateOnly, IEnumerable<TimeSlot>>? Calendar { get; set; }
        public List<DateTime>? RemainingDates { get; set; }

        public List<Region>? ServicedRegions { get; set; }

        public TimeSlot[]? AllTimeSlots { get; set; }

        public string? IdentityUserId { get; set; }

        [Required(ErrorMessage = "Изберете град или популярно място")]
        public string? SelectedSettlementId { get; set; }

        public bool IsPlace { get; set; }
    }
}
