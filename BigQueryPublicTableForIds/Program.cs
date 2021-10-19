using BigQueryPublicTableForIds.Infra.Configuration;
using BigQueryPublicTableForIds.Infra.DataStorage;
using System;
using System.Linq;

namespace BigQueryPublicTableForIds
{
    class Program
    {
        static void Main(string[] args)
        {
            StartupUtility.LoadSettings();
            Console.WriteLine("Enter choice:\r\n1.ViewRecords (arg tableName).\r\n2.InsertRecords (args fileName.csv)");
            var bqIntegrator = new GoogleBigQueryIntegrator();
            var str = Console.ReadLine();
            switch (str)
            {
                case "1":
                case "ViewRecords":
                    Console.WriteLine(string.Join(Environment.NewLine, bqIntegrator.ReadTopTenRows("bigquery-public-data.sdoh_cdc_wonder_natality.county_natality_by_maternal_morbidity")["Births"]));
                    break;
                case "2":
                case "InsertRecords":
                    Console.WriteLine("fileName.csv:");
                    var fileName = Console.ReadLine();
                    var lines = System.IO.File.ReadAllLines(fileName).Skip(1);
                    bqIntegrator.CreateTable(lines, $"articleids_{DateTime.Now:MMdd}", "an");
                    break;
                default:
                    break;
            }
            Console.WriteLine("----------");
            Console.ReadLine();

        }
    }
}
