using IpRangeCalculator.Helpers;

namespace IpRangeCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CsvStringDataHelper.IpAddressToCsv().Wait();
        }
    }
}