using System.Text.Json.Serialization;

namespace FlickrExplorer
{
    /// <summary>
    /// Dettaglio foto
    /// </summary>
    public class PhotoInfo
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Server { get; set; }
        public string DateUploaded { get; set; }
        public User Owner { get; set; }
        public PhotoContent Title { get; set; }
        public PhotoContent Description { get; set; }
        public Tags Tags { get; set; }
        public string PhotoFileName => $"{Id}_{Secret}_b.jpg";
        public string Url => $"https://live.staticflickr.com/{Server}/{PhotoFileName}";

        /// <summary>
        /// Wrapper generico per soddisfare la struttura restituita da Flickr
        /// </summary>
        public class PhotoContent
        {
            [JsonPropertyName("_content")]
            public string Content { get; set; }
        }
    }
}
