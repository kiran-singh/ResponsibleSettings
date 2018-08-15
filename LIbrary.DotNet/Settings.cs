using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Library.DotNet
{
    public abstract class Settings
    {
        public const string FormatErrorMessageMissingEnvironmentVariables = "Cannot start application. Missing environment variables: {0}";
        public const string FormatErrorMessageInvalidEnvironmentVariables = "Cannot start application. Invalid environment variables: {0}";

        protected Settings()
        {
            var declaredProperties =
                GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var appSettings = ConfigurationManager.AppSettings;

            var missingSettings =
                (from propertyInfo in declaredProperties
                 where string.IsNullOrWhiteSpace(appSettings[propertyInfo.Name])
                 select propertyInfo.Name).ToList();

            if (missingSettings.Any())
                throw new Exception(string.Format(FormatErrorMessageMissingEnvironmentVariables,
                        string.Join(", ", missingSettings)));

            var invalidSettings = new List<string>();

            foreach (var propertyInfo in declaredProperties)
            {
                var appSetting = appSettings[propertyInfo.Name];

                if (propertyInfo.PropertyType == typeof(bool))
                {
                    if(bool.TryParse(appSetting, out var value))
                        propertyInfo.SetValue(this, value, null);
                    else
                        invalidSettings.Add(propertyInfo.Name);
                }

                else if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    if(DateTime.TryParse(appSetting, out var value))
                        propertyInfo.SetValue(this, value, null);
                    else
                        invalidSettings.Add(propertyInfo.Name);
                }

                else if (propertyInfo.PropertyType == typeof(Guid))
                {
                    if(Guid.TryParse(appSetting, out var value))
                        propertyInfo.SetValue(this, value, null);
                    else
                        invalidSettings.Add(propertyInfo.Name);
                }

                else if (propertyInfo.PropertyType == typeof(int))
                {
                    if(int.TryParse(appSetting, out var value))
                        propertyInfo.SetValue(this, value, null);
                    else
                        invalidSettings.Add(propertyInfo.Name);
                }

                else 
                    propertyInfo.SetValue(this, appSetting, null);
            }
            if (invalidSettings.Any())
                throw new Exception(
                    string.Format(FormatErrorMessageInvalidEnvironmentVariables,
                        string.Join(", ", invalidSettings)));
        }
    }
}
