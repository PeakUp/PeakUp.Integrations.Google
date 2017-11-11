using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Extensions
{
    public static class GenericExtensions
    {
        /// <summary>
        /// Get class properties with description attributes. (Key: PropertyName, Value: Description)
        /// </summary>
        /// <typeparam name="T">Any class tpye</typeparam>
        /// <param name="model">Any class</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetPropertiesWithDescription<T>(this T model) where T : class
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    DescriptionAttribute authAttr = attr as DescriptionAttribute;
                    if (authAttr != null)
                    {
                        string propName = prop.Name;
                        string auth = authAttr.Description;

                        properties.Add(propName, auth);
                    }
                }
            }
            return properties;
        }

        /// <summary>
        /// Set property with name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="M"></typeparam>
        /// <param name="model">Any class</param>
        /// <param name="value">Property value type (string, int, double...)</param>
        /// <param name="propertyName">Property name</param>
        /// <returns></returns>
        public static T SetPropertyValue<T, M>(this T model, M value, string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);

            if (property == null)
                throw new Exception("Property not found!");

            property.SetValue(model, value);
            return model;
        }

        /// <summary>
        /// Get property type with name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">Any class</param>
        /// <param name="propertyName">Property name</param>
        /// <returns></returns>
        public static Type GetTypeWithPropetyName<T>(this T model,string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);

            if (property == null)
                throw new Exception("Property not found!");

            return property.PropertyType;
        }

    }
}
