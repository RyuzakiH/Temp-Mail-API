using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace TempMail.API.Extensions
{
    public static class PropertyInfoExtensions
    {

        public static Type GetPropertyType(this PropertyInfo property)
        {
            if (property.IsEnumerable())
                return property.GetEnumerablePropertyType();

            return property.PropertyType;
        }

        private static Type GetEnumerablePropertyType(this PropertyInfo property)
        {
            if (!property.IsEnumerable())
                return null;

            return property.PropertyType.GetGenericArguments()[0];
        }

        public static bool IsEnumerable(this PropertyInfo property)
        {
            return property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable));
        }
        
    }
}
