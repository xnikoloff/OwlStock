namespace OwlStock.Services.Common.HelperClasses
{
    public class TimeSlot
    {
        public TimeSlot(TimeOnly time, bool isAvailable) 
        { 
            Time = time;
            IsAvailable = isAvailable;
        }

        public TimeOnly Time { get; set; }
        public bool IsAvailable { get; set; }
    }
}
