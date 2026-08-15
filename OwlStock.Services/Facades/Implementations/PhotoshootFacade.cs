using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure.Common.EmailTemplates.Account;
using OwlStock.Infrastructure.Common.EmailTemplates.PhotoShoot;
using OwlStock.Services.Common.HelperClasses;
using OwlStock.Services.DTOs.Identity;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.DTOs.Place;
using OwlStock.Services.Facades.Interfaces;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Facades.Implementations
{
    public class PhotoshootFacade : IPhotoshootFacade
    {
        private readonly IAdministrationService _administrationService;
        private readonly IPlaceService _placeService;
        private readonly IPhotoShootService _photoShootService;
        private readonly ICalculationsService _calculationsService;
        private readonly IEmailService _emailService;
        private readonly ICalendarService _calendarService;

        public PhotoshootFacade(IAdministrationService administrationService, IPhotoShootService photoShootService, 
            IPlaceService placeService, IEmailService emailService, ICalculationsService calculationsService, ICalendarService calendarService)
        {
            _administrationService = administrationService;
            _placeService = placeService;
            _photoShootService = photoShootService;
            _emailService = emailService;
            _calculationsService = calculationsService;
            _calendarService = calendarService;
        }

        public async Task<bool> ReservePhotoshoot(CreateRegularPhotoShootDTO dto)
        {
            bool isUserSuccessful = await HandleUser(dto);

            if (!isUserSuccessful)
            {
                return false;
            }

            //create place only when user has set a name for it
            //otherwise no new place should be created
            if (!dto.UserPlace.IsNullOrEmpty())
            {
                Guid placeGuid = await HandlePlace(new CreatePlaceDTO()
                {
                    IsPopular = false,
                    Name = dto.UserPlace,
                    GoogleMapsURL = dto.GoogleMapsLink,
                    CreatedById = dto.IdentityUserId,
                    CityId = int.Parse(dto?.SelectedSettlementId ?? "")
                });
                
                dto!.PlaceId = placeGuid;
            }

            dto.Price = _calculationsService.CalculatePhotoshootPrice(dto.PhotoShootType, dto.FuelPrice, dto.NumberOfParticipants);

            Guid photoshootGuid = await _photoShootService.Add(dto);

            if (photoshootGuid == Guid.Empty)
            {
                return false;
            }

            bool areEmailsHandled = await HandleEmails(dto, photoshootGuid);

            if (!areEmailsHandled)
            {
                return false;
            }
            
            return true;
        }

        public async Task<bool> ReserveSmallProductPhotoshoot(CreateSmallProductPhotoshootDTO dto)
        {
            bool isUserSuccessful = await HandleUser(dto);

            if (!isUserSuccessful)
            {
                return false;
            }

            //create place only when user has set a name for it
            //otherwise no new place should be created
            

            dto.Price = _calculationsService.CalculatePhotoshootPrice(dto.PhotoShootType, 0, dto.NumberOfParticipants);

            Guid photoshootGuid = await _photoShootService.AddSmallProductPhotoShoot(dto);

            if (photoshootGuid == Guid.Empty)
            {
                return false;
            }

            bool areEmailsHandled = await HandleEmails(dto, photoshootGuid);

            if (!areEmailsHandled)
            {
                return false;
            }

            return true;
        }

        public async Task<Dictionary<DateOnly, IEnumerable<TimeSlot>>> GetPhotoShootsCalendar()
        {
            IEnumerable<DateTime> reservationDates = await _photoShootService.GetReservedDates();

            return _calendarService.GetPhotoShootsCalendar(reservationDates.ToList());
        }

        public async Task<bool> UpdatePhotoshoot(UpdatePhotoShootDTO dto)
        {
            //update photoshoot
            bool isUpdated = await _photoShootService.Update(dto);
            
            if (!isUpdated)
            {
                return false;
            }

            PhotoShoot? photoShoot = await _photoShootService.PhotoShootById(dto.Id);

            UpdatePhotoshootDataEmailTemplateDTO emailDTO = new()
            {
                Recipient = dto.Email,
                PhotoShootId = dto.Id,
                EmailTemplate = EmailTemplate.UpdatePhotoShoot,
                Topic = "Актуализация на фотосесия",
                PhotoshootNumber = photoShoot?.PhotoshootNumber ?? "-",
                ReservationDate = dto.ReservationDate,
                Price = dto.Price,
                PhotoShootType = photoShoot?.PhotoShootType ?? 0

           };

            await _emailService.Send(emailDTO);

            return true;
        }

        public async Task<UpdatePhotoShootDTO> GetDataForUpdate(Guid id)
        {
            PhotoShoot photoShoot = await _photoShootService.PhotoShootById(id);

            UpdatePhotoShootDTO dto = new()
            {
                DoNotUploadPhotos = photoShoot.DoNotUploadPhotos,
                IsDecidedByUs = photoShoot!.IsDecidedByUs,
                Phone = photoShoot.PersonPhone,
                Email = photoShoot.PersonEmail,
                ReservationDate = photoShoot.ReservationDate,
                TransportCustomer = photoShoot.TransportCustomer,
                PickUpAddress = photoShoot.PickUpAddress,
                Price = photoShoot.Price,
                IsSmallProduct = photoShoot.IsSmallProduct,
                PhotoDeliveryAddress = photoShoot.PhotoDeliveryAddress,
                PhotoDeliveryMethod = photoShoot.PhotoDeliveryMethod
            };

            return dto;
        }

        public async Task<IEnumerable<SetReservedDateDTO>> GetCalendarWithStatus()
        {
            List<SetReservedDateDTO> dtos = new List<SetReservedDateDTO>();
            Dictionary<DateOnly, IEnumerable <TimeSlot>> calendar = await GetPhotoShootsCalendar();

            foreach (KeyValuePair<DateOnly, IEnumerable<TimeSlot>> calendarEntry in calendar)
            {
                dtos.Add(new()
                {
                    ReservationDate = calendarEntry.Key.ToDateTime(new TimeOnly()),
                    IsAvailable = calendarEntry.Value.First().IsAvailable
                });
            }

            return dtos;
        }

        public async Task<bool> ChangeStatus(Guid id, PhotoshootStatus status)
        {
            ChangePhotoshootStatusDTO dto = await _photoShootService.ChangeStatus(id, status);

            UpdatePhotoShootEmailTemplateDTO emailDTO = new()
            {
                Recipient = dto.PersonEmail,
                PhotoShootId = dto.Id
            };

            //set status and topic for DTO
            switch (status)
            {
                case PhotoshootStatus.Completed:
                    {
                        emailDTO.EmailTemplate = EmailTemplate.UpdatePhotosForPhotoShoot;
                        emailDTO.Topic = "Страхотни новини";
                        break;
                    }

                case PhotoshootStatus.Declined:
                    {
                        emailDTO.EmailTemplate = EmailTemplate.DeclinePhotoShoot;
                        emailDTO.Topic = "Отказана фотосесия";
                        break;
                    }

                case PhotoshootStatus.Cancelled:
                    {
                        emailDTO.EmailTemplate = EmailTemplate.CancelPhotoShoot;
                        emailDTO.Topic = "Отменена фотосесия";
                        break;
                    }
            }

            return await _emailService.Send(emailDTO);
        }

        public async Task<IEnumerable<WorkingTime>> GetWorkingTime()
        {
            return await _administrationService.GetWorkingTime();
        }

        private async Task<bool> HandleUser(CreatePhotoshootDTO dto)
        {
            //get user id and email
            
            if (dto.IdentityUserId.IsNullOrEmpty())
            {
                //check if user already exists
                IdentityUser? user = await _administrationService.GetUserByEmailAsync(dto?.PersonEmail ?? "");

                if(user != null)
                {
                    dto.IdentityUserId = user.Id;
                    return true;
                }

                //else, create new user

                user = new()
                {
                    Email = dto?.PersonEmail,
                    UserName = dto?.PersonEmail
                };
                
                CreateIdentityUserDTO userDTO = await _administrationService.CreateUser(user);

                if (userDTO.Password.IsNullOrEmpty())
                {
                    return false;
                }

                //assign id of newly created user to the photoshoot DTO
                dto.IdentityUserId = user.Id;

                //assign password of newly created user to the photoshoot DTO
                dto.Password = userDTO.Password;

                return true;
            }

            return true;
        }

        private async Task<Guid> HandlePlace(CreatePlaceDTO dto)
        {
            //Returns empty when no new place is created and that breaks the workflow
            Guid placeGuid = Guid.Empty;

            //Create new place if UserPlace is not null
            if (!string.IsNullOrEmpty(dto.Name))
            {
                placeGuid = await _placeService.Create(dto);
            }

            return placeGuid;
        }

        private async Task<bool> HandleEmails(CreatePhotoshootDTO dto, Guid photoshootGuid)
        {
            PhotoShootEmailTemplateDTO emailDto = new()
            {
                Date = dto is CreateRegularPhotoShootDTO ? new DateTime
                (
                    ((CreateRegularPhotoShootDTO)dto).ReservationDate.Year, 
                    ((CreateRegularPhotoShootDTO)dto).ReservationDate.Month, 
                    ((CreateRegularPhotoShootDTO)dto).ReservationDate.Day, 
                    ((CreateRegularPhotoShootDTO)dto).ReservationTime.Hour, 
                    ((CreateRegularPhotoShootDTO)dto).ReservationTime.Minute, 0
                ) : null,
                Topic = "Успешна резервация",
                Recipient = dto.PersonEmail,
                Type = dto.PhotoShootType,
                PersonFullName = dto.PersonFirstName + " " + dto.PersonLastName,
                EmailTemplate = EmailTemplate.CreatePhotoShoot,
                Price = dto.Price,
                PhotoShootId = photoshootGuid
            };

            bool isPhotoshootEmailSent = await _emailService.Send(emailDto);

            if (!isPhotoshootEmailSent)
            {
                return false;
            }

            if (!dto.Password.IsNullOrEmpty())
            {
                CreateAccountEmailTemplateDTO accountEmailDTO = new()
                {
                    Password = dto.Password,
                    EmailTemplate = EmailTemplate.CreateAccount,
                    Topic = "Създадохме вашия профил",
                    Recipient = dto.PersonEmail
                };

                bool isAccountEmailSent = await _emailService.Send(accountEmailDTO);

                if (!isAccountEmailSent)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
