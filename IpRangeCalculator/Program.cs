using NetTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace IpRangeCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = new string[0];

            string fileName;
            while (true)
            {
                Console.WriteLine("Dosya adını giriniz.");
                fileName = Console.ReadLine();

                lines = GetTextLines(fileName);
                if (lines.Length == 0)
                {
                    Console.WriteLine("Dosa bulunamadı!");
                    continue;
                }

                List<IpRangeResult> ipRangeResults = GetIpRangeResults(lines);
                CreateCsvFile(ipRangeResults, fileName);
                Console.WriteLine("Csv dosyası oluşturuldu");
                Console.WriteLine("Programı kapatmak için bir tuşa basınız");
                Console.ReadKey();
                break;
            }
        }

        public static string[] GetTextLines(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllLines(fileName);
            }
            return new string[0];
        }
        public static List<IpRangeResult> GetIpRangeResults(string[] lines)
        {
            List<IpAddressRangeResult> reullts = new List<IpAddressRangeResult>();
            List<IpRangeResult> ipRangeResults = null;
            foreach (var line in lines)
            {
                ipRangeResults = new List<IpRangeResult>();
                IPAddressRange ipAddressRange = new IPAddressRange();
                IPAddressRange.TryParse(line, out ipAddressRange);

                reullts.Add(new IpAddressRangeResult { IPAddresses=ipAddressRange});
                //if (ipAddressRange != null)
                //    foreach (var ipAddress in ipAddressRange)
                //    {
                //        if (line.Contains("/") && (ipAddress.ToString() == ipAddressRange.Begin.ToString() || ipAddress.ToString() == ipAddressRange.End.ToString()))
                //            continue;

                //        ipRangeResults.Add(
                //            new IpRangeResult()
                //            {
                //                IpRange = line,
                //                IpAddress = ipAddress.ToString()
                //            }
                //            );
                //    }
            }
            return ipRangeResults;
        }
        public static void CreateCsvFile(List<IpRangeResult> ipRangeResults, string fileName)
        {
            string jsonData = JsonConvert.SerializeObject(ipRangeResults);

            string csvString = CsvConvertHelper.JsonToCSV(jsonData, ";");

            string csvFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.csv";

            if (File.Exists(csvFileName)) File.Delete(csvFileName);

            using (FileStream fs = new FileStream(csvFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(csvString);
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}