using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Blog.Core.Common.Helper
{
    public class AppsettingsHelper
    {
        static IConfiguration Configuration { get; set; }

        static AppsettingsHelper()
        {
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }

        public static string app(params string[] sections)
        {
            try
            {
                return Configuration[string.Join(':', sections)];
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
