using Microsoft.AspNetCore.Http;

namespace DatingApp.API.HelperClass
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Äpplication-Error", message);
            response.Headers.Add("Access-Control-Expose-Haders", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}