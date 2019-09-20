using System.Text.Encodings.Web;

namespace IdentityServer4.Extensions
{
    /// <summary>
    /// Represents a extensions for string
    /// </summary>
    public static class StringExtensions
    {
        public static string AddQueryString(this string url, string query)
        {
            if (!url.Contains("?"))
            {
                url += "?";
            }
            else if (!url.EndsWith("&"))
            {
                url += "&";
            }

            return url + query;
        }

        public static string AddQueryString(this string url, string name, string value)
        {
            return url.AddQueryString(name + "=" + UrlEncoder.Default.Encode(value));
        }
    }
}