using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    /// <summary>
    /// Retrieves the current date and/or time. Helps with unit testing by letting you mock the system clock.
    /// </summary>
    public class ClockService : IClockService
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
