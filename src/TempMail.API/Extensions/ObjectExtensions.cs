using System;
using System.Reflection;

namespace TempMail.API.Extensions
{
    public static class ObjectExtensions
    {

        public static bool HasProperty(this object obj, string propertyName, Type propertyType = null)
        {
            var type = obj.GetType();
            return (propertyType != null ? type.GetProperty(propertyName, propertyType) : type.GetProperty(propertyName)) != null;
        }

        public static bool HasMethod(this object obj, string methodName)
        {
            return obj.GetType().GetMethod(methodName) != null;
        }

        public static PropertyInfo GetPropertyByName(this object obj, string propertyName)
        {
            try
            {
                return obj.GetType().GetProperty(propertyName);
            }
            catch (AmbiguousMatchException)
            {
                return obj.GetType().GetProperty(propertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            }
        }

        public static bool SetProperty<T>(this object obj, string propertyName, object propertyValue)
        {
            PropertyInfo property = GetPropertyByName(obj, propertyName);

            if (!property.CanWrite || propertyValue == null)
                return false;

            try
            {
                property.SetValue(obj, propertyValue, null);
            }
            catch (ArgumentException)
            {
                var ty = property.GetType();

                MethodInfo method = typeof(Utilities.Utilities).GetMethod("ChangeEnumerableType");
                var newPropertyValue = method.Call(new object[] { propertyValue }, ty);

                property.SetValue(obj, newPropertyValue, null);
            }

            return true;
        }

        public static object CastTo(this object obj, Type newType)
        {
            var newObject = Activator.CreateInstance(newType);

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (HasProperty(newObject, prop.Name, type))
                {
                    PropertyInfo property = GetPropertyByName(newObject, prop.Name);

                    Type typ = property.GetType();

                    MethodInfo method = typeof(Utilities.Utilities).GetMethod("SetProperty");
                    method.Call(new object[] { newObject, prop.Name, prop.GetValue(obj, null) }, typ);
                }
            }

            return newObject;
        }

    }
}
