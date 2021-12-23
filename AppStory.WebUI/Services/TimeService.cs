using AppStory.Models;
using System;

namespace AppStory
{
    public class TimeService: ITimeService
    {
        public TimeDto Now()
        {
            return DateTime.Now.ToTimeDto();
        }

        public TimeDto Yesterday(DateTime time)
        {
            return time.AddDays(-1).ToTimeDto();
        }

        public TimeDto Tomorrow(DateTime time)
        {
            return time.AddDays(1).ToTimeDto();
        }
    }

    public interface ITimeService
    {
        TimeDto Now();

        TimeDto Yesterday(DateTime time);

        TimeDto Tomorrow(DateTime time);
    }
}
