using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;
using Google.Cloud.BigQuery.V2;
using BigQueryPublicTableForIds.Infra.Configuration;

namespace BigQueryPublicTableForIds.Infra.DataStorage
{
    public class GoogleBigQueryIntegrator
    {
        private readonly BigQueryClient _bqClient;
        private readonly string _projectId;
        private readonly string _dataSetId;

        public GoogleBigQueryIntegrator()
        {
            _projectId = StartupUtility.appSettings.ProjectId;
            _dataSetId = StartupUtility.appSettings.DataSetId;
            _bqClient = BigQueryClient.Create(_projectId);
        }

        public IDictionary<string, List<string>> ReadTopTenRows(string tableName)
        {
            var returnResponse = new Dictionary<string, List<string>>();
            var query = $@"
                SELECT * FROM `{tableName}` LIMIT 10
            ";
            var result = _bqClient.ExecuteQuery(query, parameters: null);
            foreach (var row in result)
            {
                foreach (var field in row.Schema.Fields)
                {
                    if (!returnResponse.ContainsKey(field.Name))
                        returnResponse.Add(field.Name, new List<string>());
                    returnResponse[field.Name].Add(row[field.Name].ToString());
                }
                
            }
            return returnResponse;
        }

        public void CreateTable(IEnumerable<string> articleids, string tableId, string ArticleIdColumnName)
        {
            var dataset = _bqClient.GetDataset(_dataSetId);
            var schema = new TableSchemaBuilder
            {
                { ArticleIdColumnName, BigQueryDbType.String}
            }.Build();
            var table = dataset.CreateTable(tableId: tableId, schema: schema);
            var rowsToBe = articleids.Select(x => new BigQueryInsertRow()
            {
                { ArticleIdColumnName, x}
            });
            for (int i = 0; i < rowsToBe.Count(); i=i+ 49000)
            {
                table.InsertRows(rowsToBe.Take(i+ 49000).Skip(i));
            }
            //if (rowsToBe.Count() > 50000)
            //{
            //    table.InsertRows(rowsToBe.Take(49000));
            //    table.InsertRows(rowsToBe.Skip(49000));
            //} else
            //{
            //    table.InsertRows(rowsToBe);
            //}
        }
    }
}
