using System;

namespace AppStory.Models
{
    public class TimeDto
    {
        public DateTime Time { get; set; }
    }

    public static class TimeExtensions
    {
        public static TimeDto ToTimeDto(this DateTime time)
        {
            return new TimeDto
            {
                Time = time,
            };
        }
    }
}
