using System;

namespace FoosballAPI.Services
{
    public interface IDateTimeProvider
    {
        DateTime CurrentDataTime { get; }
    }
}