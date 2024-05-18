namespace FlickrExplorer
{
    /// <summary>
    /// Oggetto foto ottenuto dalla ricerca
    /// </summary>
    public class Photo
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Server { get; set; }
        public string PhotoFileName => $"{Id}_{Secret}_n.jpg";
        public string Url => $"https://live.staticflickr.com/{Server}/{PhotoFileName}";
    }
}
