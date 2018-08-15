using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Library.Standard
{
    public class Settings
    {
        public Settings(IConfiguration dict)
        {
            var declaredProperties =
                GetType().GetTypeInfo().DeclaredProperties.ToList();

            var missingSettings =
                (from propertyInfo in declaredProperties
                    where string.IsNullOrWhiteSpace(dict[propertyInfo.Name])
                    select propertyInfo.Name).ToList();

            if (missingSettings.Any())
                throw new Exception(
                    $"Cannot start application. Missing environment variables: {string.Join(", ", missingSettings)}");

            var invalidSettings = new List<string>();

            foreach (var propertyInfo in declaredProperties)
            {
                if (propertyInfo.PropertyType == typeof(bool))
                {
                    var value = dict[propertyInfo.Name];
                    
                    if (string.IsNullOrWhiteSpace(value))
                        invalidSettings.Add(propertyInfo.Name);
                    else
                        propertyInfo.SetValue(this, value.ToBool());
                }

                else if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    var value = dict[propertyInfo.Name];
                    
                    if (string.IsNullOrWhiteSpace(value))
                        invalidSettings.Add(propertyInfo.Name);
                    else
                        propertyInfo.SetValue(this, value.ToDateTime());
                }

                else if (propertyInfo.PropertyType == typeof(Guid))
                {
                    var value = dict[propertyInfo.Name].ToGuid();

                    if (value == default(Guid))
                        invalidSettings.Add(propertyInfo.Name);
                    else
                        propertyInfo.SetValue(this, value);
                }

                else if (propertyInfo.PropertyType == typeof(int))
                {
                    var value = dict[propertyInfo.Name].ToInt();

                    if (value == default(int))
                        invalidSettings.Add(propertyInfo.Name);
                    else
                        propertyInfo.SetValue(this, value);
                }

                else
                {
                    var value = dict[propertyInfo.Name];

                    if (string.IsNullOrWhiteSpace(value))
                        invalidSettings.Add(propertyInfo.Name);
                    else
                        propertyInfo.SetValue(this, value);
                }

                if (invalidSettings.Any())
                    throw new Exception(
                        $"Cannot start application. Invalid environment variables: {string.Join(", ", invalidSettings)}");
            }
        }
    }
}
