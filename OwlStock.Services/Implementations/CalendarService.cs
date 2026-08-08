using OwlStock.Services.Common.HelperClasses;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class CalendarService : ICalendarService
    {
        private readonly int _daysCount = 366;

        private readonly DateTime _currentDateTime;
        private readonly IEnumerable<DateTime> _remainingDates;

        //Available working hours
        private readonly TimeSlot[] _timeSlots =
        {
            new(new(6, 0), true),
            new(new(7, 0), true),
            new(new(8, 0), true),
            new(new(9, 0), true),
            new(new(10, 0), true),
            new(new(11, 0), true),
            new(new(12, 0), true),
            new(new(13, 00), true),
            new(new (14, 0), true),
            new(new (15, 0), true),
            new(new(16, 0), true),
            new(new(17, 0), true),
            new(new(18, 0), true),
            new(new(19, 0), true),
            new(new(20, 0), true)
        };

        public CalendarService()
        {
            _currentDateTime = DateTime.Now;
            _remainingDates = GetRemainingDates();
        }

        /// <summary>
        /// Modifies the Calendar by adding booked dates with booked timeslots for each date
        /// </summary>
        /// <param name="reservationDates">Dates that are already booked</param>
        /// <returns>Full calendar with booked dates and booked timeslots for each date</returns>
        public Dictionary<DateOnly, IEnumerable<TimeSlot>> GetPhotoShootsCalendar(List<DateTime> reservationDates)
        {
            //build calendar
            Dictionary<DateOnly, IEnumerable<TimeSlot>> calendar = GetDefaultCalendar();
            
            //modifies the calendar
            //assigns booked timeslots for each date
            for (int i = 0; i < reservationDates.Count; i++)
            {
                //holds modified timeslots for each iteration
                //prevents changing timeslots for the other dates in the dictionary
                List<TimeSlot> modifiedTimeSlots = new();

                //assign timeslots of the booked hours for each iteration
                for (int j = 0; j < _timeSlots.Length; j++)
                {
                    modifiedTimeSlots.Add(new(_timeSlots[j].Time, _timeSlots[j].IsAvailable));
                }
                
                //make it not available
                for (int j = 0; j < modifiedTimeSlots.Count; j++)
                {
                    modifiedTimeSlots[j].IsAvailable = false;
                }

                //assign the modified timeslots to the key for the current reservation
                calendar[DateOnly.FromDateTime(reservationDates[i].Date)] = modifiedTimeSlots;
            }

            return calendar;
        }

        //Expose private {_timeSlots} to external services
        public TimeSlot[] GetTimeSlots()
        {
            TimeSlot[] timeSlots = new TimeSlot[_timeSlots.Length];

            for (int i = 0; i < timeSlots.Length; i++)
            {
                timeSlots[i] = _timeSlots[i];
                timeSlots[i].IsAvailable = _timeSlots[i].IsAvailable;
            }

            return timeSlots;
        }

        /// <summary>
        /// Builds Dictionary of dates
        /// starting from today and timelosts for each date 
        /// </summary>
        /// <returns>Dictionary of date and timelosts for each date</returns>
        public Dictionary<DateOnly, IEnumerable<TimeSlot>> GetDefaultCalendar()
        {
            Dictionary<DateOnly, IEnumerable<TimeSlot>> calendar = new();
            //convert {_remainingDates} to List
            List<DateTime> remainingDatesList = _remainingDates.ToList();

            //build Dictionary of dates and available timeslots for each date
            for (int i = 0; i < remainingDatesList.Count; i++)
            {
                calendar.Add(DateOnly.FromDateTime(remainingDatesList[i]), _timeSlots);
            }

            return calendar;
        }

        /// <summary>
        /// Gets the remaing {_daysCount} days from the current date on
        /// </summary>
        /// <returns>IEnumerable<DateTime></returns>
        public IEnumerable<DateTime> GetRemainingDates()
        {
            List<DateTime> remainingDates = new();
            
            for(int i = 0; i < _daysCount; i++)
            {
                if(i >= _daysCount)
                {
                    break;
                }
                remainingDates.Add(_currentDateTime.AddDays(i));
            }

            return remainingDates;
        }

        /*public int GetStartingDayOfWeek()
        {
            DateTime dateTime = new(_currentDateTime.Year, _currentDateTime.Month, 1);
            return (int)dateTime.DayOfWeek;
        }*/

        /*public IEnumerable<TimeSlot> ConvertToTimeSlot(IEnumerable<DateTime> dateTimes, bool isAvaialble)
        {
            List<TimeSlot> timeOnlies = new();

            foreach (DateTime dateTime in dateTimes)
            {
                timeOnlies.Add(new(new(dateTime.Hour, dateTime.Minute), isAvaialble));
            }

            return timeOnlies;
        }*/
    }
}
