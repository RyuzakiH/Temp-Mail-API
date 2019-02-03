using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HtmlAgilityPack;

namespace TempMail.API
{
    public static class Utilities
    {

        public static Type GetPropertyType(PropertyInfo property)
        {
            return property.PropertyType;
        }

        public static Type GetEnumerablePropertyType(PropertyInfo property)
        {
            if (!IsEnumerable(property))
                return null;
            return property.PropertyType.GetGenericArguments()[0];
        }

        public static bool IsEnumerable(PropertyInfo property)
        {
            return property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable));
        }


        public static bool SetProperty<T>(object obj, string propertyName, object propertyValue)
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
                var ty = GetEnumerablePropertyType(property);

                MethodInfo method = typeof(Utilities).GetMethod("ChangeEnumerableType");
                var newPropertyValue = CallGenericMethodByReflection(method, new object[] { propertyValue }, ty);

                property.SetValue(obj, newPropertyValue, null);
            }

            return true;
        }


        public static IEnumerable ChangeEnumerableType<T>(IEnumerable enumerable)
        {
            return enumerable.Cast<T>();
        }

        public static PropertyInfo GetPropertyByName(object obj, string propertyName)
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


        public static bool HasProperty(object obj, string propertyName, Type propertyType = null)
        {
            var type = obj.GetType();
            return (propertyType != null ? type.GetProperty(propertyName, propertyType) : type.GetProperty(propertyName)) != null;
        }

        public static bool HasMethod(object obj, string methodName)
        {
            return obj.GetType().GetMethod(methodName) != null;
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


        public static object CallGenericMethodByReflection(MethodInfo method, object[] parameters, params Type[] typeArguments)
        {
            MethodInfo generic = method.MakeGenericMethod(typeArguments);
            return generic.Invoke(null, BindingFlags.CreateInstance, null, parameters, System.Globalization.CultureInfo.CurrentCulture);
        }

        public static object CastObjectToAnotherType(object obj, Type newType)
        {
            var newObject = Activator.CreateInstance(newType);

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (HasProperty(newObject, prop.Name, type))
                {
                    PropertyInfo property = GetPropertyByName(newObject, prop.Name);

                    Type typ = GetPropertyType(property);

                    MethodInfo method = typeof(Utilities).GetMethod("SetProperty");
                    CallGenericMethodByReflection(method, new object[] { newObject, prop.Name, prop.GetValue(obj, null) }, typ);
                }
            }

            return newObject;
        }

    }
}
