using System;
using System.Reflection;

namespace TempMail.API.Extensions
{
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Calls generic method using reflection
        /// </summary>
        public static object Call(this MethodInfo method, object[] parameters, params Type[] typeArguments)
        {
            MethodInfo generic = method.MakeGenericMethod(typeArguments);
            return generic.Invoke(null, BindingFlags.CreateInstance, null, parameters, System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
