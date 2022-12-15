using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace IpRangeCalculator
{
    public static class CsvConvertHelper
    {
        public static string JsonToCSV(string jsonContent, string delimiter)
        {
            var csvConfiguration = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = delimiter,
                Encoding = Encoding.UTF8,
            };
            StringWriter csvString = new StringWriter();
            using (var csv = new CsvWriter(csvString, csvConfiguration))
            {


                using (var dt = jsonStringToTable(jsonContent))
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        csv.WriteField(column.ColumnName);
                    }
                    csv.NextRecord();

                    foreach (DataRow row in dt.Rows)
                    {
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                            csv.WriteField(row[i]);
                        }
                        csv.NextRecord();
                    }
                }
            }
            return csvString.ToString();
        }

        private static DataTable jsonStringToTable(string jsonContent)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonContent);
            return dt;
        }
    }
}
