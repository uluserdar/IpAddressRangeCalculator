using IpRangeCalculator.Helpers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IpRangeCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IpAddressToCsv().Wait();
        }

        static async Task IpAddressToCsv()
        {
            string[] lines = new string[0];
            StringBuilder csvStringData = new StringBuilder();

            string fileName;
            while (true)
            {
                Console.WriteLine("Enter file name.");
                fileName = Console.ReadLine();

                csvStringData.AppendLine("Ip Range;Ip Address");
                try
                {
                    await csvStringData.CreateIpAddressListAsync(fileName);
                    await csvStringData.CreateCsvFileAsync(fileName);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    continue;
                }

                Console.WriteLine("CSV file created.");
                Console.WriteLine("Press any key to close the program");
                Console.ReadKey();
                break;
            }
        }
    }
}