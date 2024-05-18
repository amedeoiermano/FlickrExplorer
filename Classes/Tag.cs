namespace FlickrExplorer
{
    public class Tags
    {
        public List<Tag> Tag { get; set; }
    }
    public class Tag
    {
        public string Id { get; set; }
        public string Raw { get; set; }
        /// <summary>
        /// Formatta il tag con il cancelletto (se non presente)
        /// </summary>
        public string HashTag => string.IsNullOrEmpty(Raw) ? string.Empty : (!Raw.StartsWith("#") ? $"#{Raw}" : Raw);
    }
}
