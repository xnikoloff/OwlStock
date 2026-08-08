using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class GiftCardService : IGiftCardService
    {
        private PhotonicDbContext _context;
        private ILogger<GiftCardService> _logger;

        public GiftCardService(PhotonicDbContext context, ILogger<GiftCardService> logger) 
        {
            _context = context ?? new PhotonicDbContext();
            _logger = logger;
        }

        public async Task<IEnumerable<GiftCard>> GetAll()
        {
            if (_context.GiftCards is null)
            {
                _logger.LogError($"{nameof(_context.GiftCards)} is null at {DateTime.UtcNow} in {nameof(GiftCardService)}, {nameof(GetAll)}");
                return new List<GiftCard>();
            }

            try
            {
                return await _context.GiftCards
                    .OrderByDescending(gf => gf.Id)
                    .ToListAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new List<GiftCard>();
            }
        }

        public async Task<GiftCard> GetById(Guid id)
        {
            if (_context.GiftCards is null)
            {
                _logger.LogError($"{nameof(_context.GiftCards)} is null at {DateTime.UtcNow} in {nameof(GiftCardService)}, {nameof(GetById)}");
                return new();
            }

            if(id == Guid.Empty)
            {
                _logger.LogError($"{nameof(id)} is empty at {DateTime.UtcNow} in {nameof(GiftCardService)}, {nameof(GetById)}");
                return new();
            }

            try
            {
               return await _context.GiftCards.FindAsync(id) ?? new GiftCard();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }

        public async Task<bool> Create(GiftCard giftCard)
        {
            if(giftCard == null)
            {
                _logger.LogError($"{nameof(giftCard)} is null at {DateTime.UtcNow} in {nameof(GiftCardService)}, {nameof(Create)}");
                return false;
            }

            if (giftCard.Receiver.IsNullOrEmpty())
            {
                _logger.LogError($"${nameof(giftCard.Receiver)} is null or empty at {DateTime.UtcNow} in {nameof(GiftCardService)}, {nameof(Create)}");
                return false;
            }

            giftCard.CreatedOn = DateTime.Now;
            giftCard.GiftCardNumber = GenerateGiftCardNumber(giftCard.PhotoShootType);
            giftCard.Status = GiftCardStatus.New;

            try
            {
                await _context.AddAsync(giftCard);
                await _context.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return false;
            }
        }

        public async Task<bool> ChangeStatus(Guid id, GiftCardStatus status)
        {
            if (_context.GiftCards is null)
            {
                _logger.LogError($"{nameof(_context.GiftCards)} is null at {DateTime.UtcNow} in {nameof(GiftCardService)}, {nameof(GetById)}");
                return false;
            }

            if (id == Guid.Empty)
            {
                _logger.LogError($"{nameof(id)} is empty at {DateTime.UtcNow} in {nameof(GiftCardService)}, {nameof(GetById)}");
                return false;
            }

            if(status > 0)
            {
                try
                {
                    GiftCard? giftCard = await _context.GiftCards.FindAsync(id);

                    if (giftCard != null) 
                    {
                        giftCard.Status = status;
                        await _context.SaveChangesAsync();
                        return true;
                    }

                    else
                    {
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        
        private string GenerateGiftCardNumber(PhotoShootType photoShootType)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //generate three random numbers to get three random letters from the alphabet
            Random random = new();
            int randomNumber1 = random.Next(0, alphabet.Length);
            int randomNumber2 = random.Next(0, alphabet.Length);
            int randomNumber3 = random.Next(0, alphabet.Length);

            try
            {
                string number =
                    "GC-" +
                    DateTime.Now.Year.ToString()[2..] +
                    DateTime.Now.Month +
                    DateTime.Now.Day + "-" +
                    photoShootType.ToString()[..3].ToUpper() + "-" +
                    Guid.NewGuid().ToString().ToUpper()[..4] + "-" +
                    alphabet[randomNumber1] + alphabet[randomNumber2] + alphabet[randomNumber3];

                return number;
            }

            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return string.Empty;
            }
        }
    }
}
