using System;

namespace FoosballAPI.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime CurrentDataTime => DateTime.UtcNow;
    }
}
