using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class TestimonyService : ITestimonyService
    {
        private readonly PhotonicDbContext _context;
        private readonly ILogger<TestimonyService> _logger;

        public TestimonyService(PhotonicDbContext context, ILogger<TestimonyService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Creates new user testimony
        /// </summary>
        /// <param name="testimony">Testimony entity containing the required data</param>
        /// <returns></returns>
        public async Task<Testimony> Create(Testimony testimony)
        {
            testimony.CreatedOn = DateTime.Now;
            testimony.IsApproved = false;
            testimony.IsHidden = false;
            
            await _context.AddAsync(testimony);
            await _context.SaveChangesAsync();

            return testimony;
        }

        /// <summary>
        /// Approves a testimony
        /// </summary>
        /// <param name="id">Id of the testimony</param>
        /// <returns>The testimony</returns>
        /// <exception cref="NullReferenceException">Thrown when the testimony cannot be found</exception>
        public async Task<Testimony> Approve(Guid id)
        {
            Testimony? testimony = await _context.Testimonies
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (testimony == null)
            {
                throw new NullReferenceException($"{nameof(testimony)} with Id {id} cannot be found");
            }

            testimony.IsApproved = true;
            testimony.ApprovedOn = DateTime.Now;
            await _context.SaveChangesAsync();

            return testimony;
        }

        /// <summary>
        /// Sets the IsHidden to true
        /// </summary>
        /// <param name="id">Id of the testimony</param>
        /// <returns>The testimony</returns>
        /// <exception cref="NullReferenceException">Thrown when the testimony cannot be found</exception>
        public async Task<Testimony> Hide(Guid id)
        {
            Testimony? testimony = await _context.Testimonies
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (testimony == null)
            {
                throw new NullReferenceException($"{nameof(testimony)} with Id {id} cannot be found");
            }

            testimony.IsHidden = true;
            testimony.HiddenOn = DateTime.Now;
            await _context.SaveChangesAsync();

            return testimony;
        }

        /// <summary>
        /// Sets the IsHidden to false for a testimony has already had its IsHidden set to true
        /// </summary>
        /// <param name="id">Id of the testimony</param>
        /// <returns>The testimony</returns>
        /// <exception cref="NullReferenceException">Thrown when the testimony cannot be found</exception>
        public async Task<Testimony> Unhide(Guid id)
        {
            Testimony? testimony = await _context.Testimonies
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (testimony == null)
            {
                throw new NullReferenceException($"{nameof(testimony)} with Id {id} cannot be found");
            }

            testimony.IsHidden = false;
            testimony.UnhiddenOn = DateTime.Now;
            await _context.SaveChangesAsync();

            return testimony;
        }

        /// <summary>
        /// Gets the last four created testimonies
        /// </summary>
        /// <returns>List of the testimonies</returns>
        public async Task<IEnumerable<Testimony>> GetLastFour()
        {
            try
            {
                int count = await _context.Testimonies.CountAsync();

                //if count is lower than 4 take all
                if (count < 4)
                {
                    return await _context.Testimonies
                        .Where(t => t.IsHidden == false && t.IsApproved)
                        .OrderByDescending(t => t.CreatedOn)
                        .ToListAsync();
                }

                //if count is bigger than 4, take only 4
                return await _context.Testimonies
                        .Where(t => t.IsHidden == false && t.IsApproved)
                        .OrderByDescending(t => t.CreatedOn)
                        .Take(4)
                        .ToListAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new List<Testimony>();
            }
        }

        /// <summary>
        /// Gets all testimonies that have property IsApproved set to true and IsHidden to false
        /// </summary>
        /// <returns>List of Testimony</returns>
        public async Task<IEnumerable<Testimony>> GetApproved()
        {
            return await _context.Testimonies
                .Where(t => t.IsHidden == false && t.IsApproved)
                .OrderByDescending(t => t.CreatedOn)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all testimonies that have property IsHidden set to true
        /// </summary>
        /// <returns>List of testimonies</returns>
        public async Task<IEnumerable<Testimony>> GetHidden()
        {
            return await _context.Testimonies
                .Where(t => t.IsHidden)
                .OrderByDescending(t => t.CreatedOn)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all testimonies that have properties IsHidden and IsApproved set to false
        /// </summary>
        /// <returns>List of Testimony</returns>
        public async Task<IEnumerable<Testimony>> GetNew()
        {
            return await _context.Testimonies
                .Where(t => t.IsHidden == false && t.IsApproved == false)
                .OrderByDescending(t => t.CreatedOn)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all testimonies have property IsHidden set to false and HiddenOn that is not null, meaning that they were hidden at least once before
        /// </summary>
        /// <returns>List of Testimony</returns>
        public async Task<IEnumerable<Testimony>> GetUnhidden()
        {
            return await _context.Testimonies
                .Where(t => t.IsHidden == false && t.HiddenOn != null)
                .OrderByDescending(t => t.CreatedOn)
                .ToListAsync();
        }
    }
}
