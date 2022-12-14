using IpAddreassRangeCalculator;
using NetTools;
using Newtonsoft.Json;
using System.Text;

string[] lines = new string[0];

string fileName;
while (true)
{
    Console.WriteLine("Dosya adını giriniz.");
    fileName = Console.ReadLine();

    if (File.Exists(fileName))
    {
        lines = File.ReadAllLines(fileName);
        break;
    }

    Console.WriteLine("Dosa bulunamadı!");
}

List<IpRangeResult> ipRangeResults = new List<IpRangeResult>();

foreach (var line in lines)
{
    IPAddressRange ipAddressRange = new IPAddressRange();
    IPAddressRange.TryParse(line, out ipAddressRange);

    if (ipAddressRange != null)
        foreach (var ipAddress in ipAddressRange)
        {
            string[] numbers = ipAddress.ToString().Split('.');
            string lasnNumber = numbers[numbers.Length - 1];

            if (lasnNumber != "0")
                ipRangeResults.Add(
                    new IpRangeResult()
                    {
                        IpRange = line,
                        IpAddress = ipAddress.ToString()
                    }
                    );
        }
}

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
Console.WriteLine("Csv dosyası oluşturuldu");
Console.ReadLine();

