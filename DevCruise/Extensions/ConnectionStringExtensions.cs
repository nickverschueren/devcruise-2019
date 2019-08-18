using System.IO;

namespace Euricom.DevCruise.Extensions
{
    public static class ConnectionStringExtensions
    {
        public static string FormatAppDataPath(this string connectionString, string contentRootPath)
            => connectionString.Replace("|DataDirectory|",
                Path.Combine(contentRootPath, "App_Data"));
    }
}
