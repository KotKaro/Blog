using Microsoft.Extensions.Configuration;

namespace Blog.API.Common
{
    public static class ConfigurationExtensions
    {
        public static string GetUsername(this IConfiguration configuration)
        {
            return configuration["blog:username"];
        }

        public static string GetPassword(this IConfiguration configuration)
        {
            return configuration["blog:password"];
        }
    }
}