using System;
using System.Net;

namespace TempMail.API.Extensions
{
    internal static class CookieContainerExtensions
    {
        internal static Cookie GetCookie(this CookieContainer cookieContainer, Uri uri, string cookieName) =>
            cookieContainer.GetCookies(uri)[cookieName];

        internal static Cookie GetCookie(this CookieContainer cookieContainer, string uri, string cookieName) =>
            cookieContainer.GetCookies(new Uri(uri))[cookieName];


        internal static void SetCookie(this CookieContainer cookieContainer, Uri uri, string cookieHeader) =>
            cookieContainer.SetCookies(uri, cookieHeader);

        internal static void SetCookie(this CookieContainer cookieContainer, string uri, string cookieHeader) =>
            cookieContainer.SetCookies(new Uri(uri), cookieHeader);


        internal static void SetCookie(this CookieContainer cookieContainer, Uri uri, string cookieName, string cookieValue) =>
            cookieContainer.SetCookies(uri, $"{cookieName}={cookieValue}");

        internal static void SetCookie(this CookieContainer cookieContainer, string uri, string cookieName, string cookieValue) =>
            cookieContainer.SetCookies(new Uri(uri), $"{cookieName}={cookieValue}");
    }
}