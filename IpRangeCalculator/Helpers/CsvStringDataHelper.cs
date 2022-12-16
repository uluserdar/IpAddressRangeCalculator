using NetTools;
using SimpleConsoleProgress;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpRangeCalculator.Helpers
{
    public static class CsvStringDataHelper
    {
        public static async Task CreateIpAddressListAsync(this StringBuilder csvStringData, string fileName)
        {
            await Task.Run(() =>
            {
                string[] lines = new string[0];
                if (File.Exists(fileName)) lines = File.ReadAllLines(fileName);
                else throw new FileNotFoundException("File not found!");
                Console.WriteLine($":Total IP Range {lines.Length}");
                for (int i = 0; i < lines.Length; i++)
                {
                    IPAddressRange ipAddressRange = new IPAddressRange();
                    IPAddressRange.TryParse(lines[i], out ipAddressRange);
                    if (ipAddressRange != null)
                    {
                        Console.WriteLine($"Processing IP range: {i+1}/{lines.Length}");
                        Console.WriteLine($"IP Range: {ipAddressRange.ToCidrString()} - Start IP: {ipAddressRange.Begin} - End IP: {ipAddressRange.End}");
                        Console.WriteLine($"{ipAddressRange.ToCidrString()} processing...");
                        int ipAddressIndex = 0;
                        int ipAddressCount = ipAddressRange.AsEnumerable().Count();
                        foreach (var ipAddress in ipAddressRange)
                        {
                            if (lines[i].Contains("/") && (ipAddress.ToString() == ipAddressRange.Begin.ToString() || ipAddress.ToString() == ipAddressRange.End.ToString()))
                                continue;
                            csvStringData.AppendLine($"{lines[i]};{ipAddress.ToString()}");
                            ipAddressIndex++;
                            Progress.Write(ipAddressIndex, ipAddressCount);
                            GC.Collect();
                        }
                        Console.WriteLine($"{ipAddressRange.ToCidrString()} done");
                    }
                }
            });
        }
        public static async Task CreateCsvFileAsync(this StringBuilder csvStringData, string fileName)
        {
            await Task.Run(() =>
            {
                string csvFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.csv";

                if (File.Exists(csvFileName)) File.Delete(csvFileName);

                using (FileStream fs = new FileStream(csvFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(csvStringData.ToString());
                        fs.Write(buffer, 0, buffer.Length);
                    }
                }
            });
        }
    }
}