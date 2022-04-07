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
            
            var bqIntegrator = new GoogleBigQueryIntegrator();

            if (args.Length == 0)
            {
                Console.WriteLine("Enter choice:\r\n1.ViewRecords (arg tableName).\r\n2.InsertRecords (args fileName.csv)");
                UserInput(bqIntegrator);
                Finish();
                Console.ReadLine();
                return;
            }

            if (args[0]=="1")
            {
                View(bqIntegrator);
            } else
            {
                if(args.Length>2)
                    Write(bqIntegrator, args[1], args[2]);
                else
                    Write(bqIntegrator, args[1]);
            }
            Finish();


        }

        private static void Finish()
        {
            Console.WriteLine("----------");
        }

        private static void UserInput(GoogleBigQueryIntegrator bqIntegrator)
        {
            var str = Console.ReadLine();
            switch (str)
            {
                case "1":
                case "ViewRecords":
                    View(bqIntegrator);
                    break;
                case "2":
                case "InsertRecords":
                    Console.WriteLine("fileName.csv:");
                    var fileName = Console.ReadLine();
                    Write(bqIntegrator, fileName);
                    break;
                default:
                    break;
            }
        }

        private static void Write(GoogleBigQueryIntegrator bqIntegrator, string fileName, string postfix="")
        {
            var lines = System.IO.File.ReadAllLines(fileName).Skip(1);
            bqIntegrator.CreateTable(lines, $"articleids_{DateTime.Now:MMdd}{postfix}", "an");
        }

        private static void View(GoogleBigQueryIntegrator bqIntegrator)
        {
            Console.WriteLine(string.Join(Environment.NewLine, bqIntegrator.ReadTopTenRows("bigquery-public-data.sdoh_cdc_wonder_natality.county_natality_by_maternal_morbidity")["Births"]));
        }
    }
}
