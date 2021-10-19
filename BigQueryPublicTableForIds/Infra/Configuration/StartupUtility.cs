using BigQueryPublicTableForIds.Domain.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BigQueryPublicTableForIds.Infra.Configuration
{
    class StartupUtility
    {
        public static BigQuerySettings appSettings = new BigQuerySettings();
        public static void LoadSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",  optional: true, reloadOnChange : true);
            var configuration = builder.Build();
            ConfigurationBinder.Bind(configuration.GetSection("BigQuerySettings"), appSettings);

        }
    }
}
