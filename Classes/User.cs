namespace FlickrExplorer
{
    /// <summary>
    /// Descrive un utente Flickr
    /// </summary>
    public class User
    {
        public string Nsid { get; set; }
        public string Username { get; set; }
        public string IconServer { get; set; }
        public string IconUrl => $"https://farm66.staticflickr.com/{IconServer}/buddyicons/{Nsid}.jpg";
        public int IconFarm { get; set; }
    }
}
