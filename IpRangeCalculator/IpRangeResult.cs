using System;

namespace IpRangeCalculator
{
    public class IpRangeResult:IDisposable
    {
        public string IpRange { get; set; }
        public string IpAddress { get; set; }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}