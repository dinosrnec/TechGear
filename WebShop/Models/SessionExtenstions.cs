using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace WebShop
{
    public static class SessionExtensions
    {
        // Pretvara listu proizvoda u tekst i sprema u sesiju
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Čita tekst iz sesije i pretvara ga natrag u listu proizvoda
        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}