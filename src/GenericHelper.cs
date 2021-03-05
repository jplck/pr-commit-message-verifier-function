using Microsoft.Extensions.Configuration;
using System;

namespace PRCommitStatusCheck
{
    class GenericHelper
    {
        public static IConfigurationRoot GetConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddEnvironmentVariables()
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
