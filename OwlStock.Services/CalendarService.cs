using Microsoft.EntityFrameworkCore;
using OwlStock.Services.Common.HelperClasses;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly int _daysCount = 366;

        private readonly DateTime _currentDateTime;
        private readonly IEnumerable<DateTime> _remainingDates;

        private readonly TimeSlot[] _timeSlots =
        {
            new(new(8, 0), true),
            new(new(10, 0), true),
            new(new(12, 00), true),
            new(new (14, 0), true),
            new(new (16, 0), true),
            new(new(18, 0), true),
            new(new(20, 0), true)
        };

        public CalendarService()
        {
            _currentDateTime = DateTime.Now;
            _remainingDates = GetRemainingDates();
        }

        public Dictionary<DateOnly, IEnumerable<TimeSlot>> GetPhotoShootsCalendar(List<DateTime> reservationDates)
        {
            Dictionary<DateOnly, IEnumerable<TimeSlot>> calendar = GetDefaultCalendar();
            List<DateTime> modifiedDates = new();
            TimeSlot[] timeSlots = GetTimeSlots().ToArray();
            
            for (int i = 0; i < reservationDates.Count; i++)
            {
                List<TimeSlot> modifiedTimeSlots = new();

                //assign new objects for the booked hours for differented dates
                for (int j = 0; j < timeSlots.Length; j++)
                {
                    modifiedTimeSlots.Add(new(timeSlots[j].Time, timeSlots[j].IsAvailable));
                }
                
                //make it not available
                for (int j = 0; j < modifiedTimeSlots.Count; j++)
                {
                    modifiedTimeSlots[j].IsAvailable = false;
                }

                //assign the modified timeslots to the key for the current reservation
                calendar[DateOnly.FromDateTime(reservationDates[i].Date)] = modifiedTimeSlots;

                //Add modified dates to check if the date has already been modified
                modifiedDates.Add(reservationDates[i]);
            }

            return calendar;
        }

        //remove, it's the same as the readonly {_timeSlots} array
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

        public Dictionary<DateOnly, IEnumerable<TimeSlot>> GetDefaultCalendar()
        {
            Dictionary<DateOnly, IEnumerable<TimeSlot>> calendar = new();
            

            for(int i = 0; i < GetRemainingDates().Count(); i++)
            {
                calendar.Add(DateOnly.FromDateTime(_remainingDates.ToList()[i]), _timeSlots);
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
                remainingDates.Add(_currentDateTime.AddDays(i + 1));
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
