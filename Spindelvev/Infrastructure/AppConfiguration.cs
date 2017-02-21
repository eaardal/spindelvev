using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Spindelvev.Infrastructure
{
    public class AppConfiguration : IAppConfiguration
    {
        public IEnumerable<string> HostnameFilters => ReadConfigurationAsEnumerable<string>("Filter.Hostnames");
        public IEnumerable<string> RouteFilters => ReadConfigurationAsEnumerable<string>("Filter.Routes");

        private static IEnumerable<T> ReadConfigurationAsEnumerable<T>(string key, bool throwIfNoValue = false, T defaultValue = default(T))
        {
            var value = ReadConfiguration(key, throwIfNoValue, string.Empty);
            return value.Split(';').Select(item => (T)Convert.ChangeType(item, typeof(T)));
        }

        private static T ReadConfiguration<T>(string key, bool throwIfNoValue = false, T defaultValue = default(T))
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                if (!string.IsNullOrEmpty(value))
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                if (throwIfNoValue)
                {
                    throw new Exception(@"Error fetching value from config: {key}");
                }
                return defaultValue;
            }
            catch (Exception ex)
            {
                if (throwIfNoValue)
                {
                    throw new Exception(@"Error fetching value from config: {key}", ex);
                }
                return defaultValue;
            }
        }
    }
}