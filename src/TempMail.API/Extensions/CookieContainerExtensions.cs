using System;
using System.Net;

namespace TempMail.API.Extensions
{
    internal static class CookieContainerExtensions
    {
        internal static Cookie GetCookie(this CookieContainer cookieContainer, Uri uri, string cookieName) =>
            cookieContainer.GetCookies(uri)[cookieName];

        internal static Cookie GetCookie(this CookieContainer cookieContainer, string uri, string cookieName) =>
            cookieContainer.GetCookie(new Uri(uri), cookieName);


        internal static void AddCookie(this CookieContainer cookieContainer, Uri uri, string cookieHeader) =>
            cookieContainer.SetCookies(uri, cookieHeader);

        internal static void AddCookie(this CookieContainer cookieContainer, string uri, string cookieHeader) =>
            cookieContainer.AddCookie(new Uri(uri), cookieHeader);


        internal static void AddCookie(this CookieContainer cookieContainer, Uri uri, string cookieName, string cookieValue) =>
            cookieContainer.SetCookies(uri, $"{cookieName}={cookieValue}");

        internal static void AddCookie(this CookieContainer cookieContainer, string uri, string cookieName, string cookieValue) =>
            cookieContainer.AddCookie(new Uri(uri), cookieName, cookieValue);


        internal static void SetCookie(this CookieContainer cookieContainer, Uri uri, string cookieName, string cookieValue)
        {
            var cookie = cookieContainer.GetCookie(uri, cookieName);
            
            if (cookie != null)
                cookie.Value = cookieValue;
            else
                cookieContainer.AddCookie(uri, $"{cookieName}={cookieValue}");
        }

        internal static void SetCookie(this CookieContainer cookieContainer, string uri, string cookieName, string cookieValue) =>
            cookieContainer.SetCookie(new Uri(uri), cookieName, cookieValue);
    }
}