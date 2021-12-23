using System;

namespace AppStory
{
    public class AppStoryException : Exception
    {
        public AppStoryException(string message) : base(message)
        {
        }
    }
}
