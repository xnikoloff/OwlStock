using OwlStock.Services.Common.HelperClasses;

namespace OwlStock.Services.Interfaces
{
    public interface ICalendarService
    {
        /// <summary>
        /// Returns the remaining dates of the current year as each date contains a list of timeslots
        /// </summary>
        /// <returns>Dictionary of Date with Array of TimeSlot</returns>
        Dictionary<DateOnly, IEnumerable<TimeSlot>> GetDefaultCalendar();
        Dictionary<DateOnly, IEnumerable<TimeSlot>> GetPhotoShootsCalendar(List<DateTime> resevationDates);
        TimeSlot[] GetTimeSlots();
        //int GetStartingDayOfWeek();
        IEnumerable<DateTime> GetRemainingDates();
        //IEnumerable<TimeSlot> ConvertToTimeSlot(IEnumerable<DateTime> dateTimes, bool isAvailalbe);
        
        
    }
}
