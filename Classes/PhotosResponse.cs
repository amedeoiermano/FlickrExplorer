namespace FlickrExplorer.Classes
{
    /// <summary>
    /// Oggetto restituito da ricerca foto
    /// </summary>
    public class PhotosResponse : ApiResponse
    {
        public SearchPhotosResponseData Photos { get; set; }
    }

    public class SearchPhotosResponseData
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public IEnumerable<Photo> Photo { get; set; }
    }
}
