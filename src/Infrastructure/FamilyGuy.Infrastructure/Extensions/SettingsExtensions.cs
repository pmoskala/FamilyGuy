using Microsoft.Extensions.Configuration;

namespace FamilyGuy.Infrastructure.Extensions
{
    public static class SettingsExtensions
    {
        public static T GetSettings<T>(this IConfiguration configuration) where T : new()
        {
            string section = typeof(T).Name.Replace("Settings", string.Empty);
            T configurationValue = new T();
            IConfigurationSection configurationSection = configuration.GetSection(section);
            configurationSection.Bind(configurationValue);

            return configurationValue;
        }
    }
}
