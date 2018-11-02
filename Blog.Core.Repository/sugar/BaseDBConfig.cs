
using Blog.Core.Common.Helper;

namespace Blog.Core.Repository
{
    public class BaseDBConfig
    {
        public static string ConnectionString { get; set; } = AppsettingsHelper.app("AppSettings", "SqlServer", "SqlServerConnection");
    }
}