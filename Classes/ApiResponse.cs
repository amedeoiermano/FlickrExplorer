using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace FlickrExplorer
{
    /// <summary>
    /// Wrapper risposte chiamate api contenente stato e messaggio opzionale
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Stato richiesta
        /// </summary>
        [JsonPropertyName("stat")]
        public Status Status { get; set; }
        /// <summary>
        /// Dettagli opzionali sull'esito della richiesta
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Converte la stringa restituita dalle api Flickr nell'oggetto atteso
        /// </summary>
        /// <typeparam name="T">Tipo oggetto atteso</typeparam>
        /// <param name="apiResponseText">Stringa restituita dall'api</param>
        /// <returns>Oggetto convertito da stringa restituita dall'api Flickr</returns>
        /// <exception cref="Exception"></exception>
        public static T GetData<T>(string apiResponseText)
        {
            // estrae JSON dalla risposta dell'api Flickr (che avrà formato:jsonFlickrApi({...}))
            Match match = new Regex(@"jsonFlickrApi\((.*?)\}\)").Match(apiResponseText);
            if (match.Success)
            {
                StringBuilder b = new StringBuilder();
                b.Append(match.Groups[1].Value.Trim());

                // ripristina carattere finale "}"
                b.Append("}");

                // restituisce oggetto da JSON
                return JsonSerializer.Deserialize<T>(b.ToString(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            throw new Exception($"Impossibile estrarre dati da api");
        }
    }

    /// <summary>
    /// Stati gestiti in risposta dalle api Flickr
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        Ok,
        Fail
    }
}
