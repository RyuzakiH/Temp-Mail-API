using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using TempMail.API.Extensions;

namespace TempMail.API.Utilities
{
    public static class Utilities
    {

        public static IEnumerable ChangeEnumerableType<T>(IEnumerable enumerable)
        {
            return enumerable.Cast<T>();
        }
      
        public static Type GetTypeByName(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }

        public static bool SetProperty<T>(this object obj, string propertyName, object propertyValue)
        {
            PropertyInfo property = obj.GetPropertyByName(propertyName);

            if (!property.CanWrite || propertyValue == null)
                return false;

            try
            {
                property.SetValue(obj, propertyValue, null);
            }
            catch (ArgumentException)
            {
                var ty = property.GetPropertyType();

                MethodInfo method = typeof(Utilities).GetMethod("ChangeEnumerableType");
                var newPropertyValue = method.Call(new object[] { propertyValue }, ty);

                property.SetValue(obj, newPropertyValue, null);
            }

            return true;
        }

    }
}
