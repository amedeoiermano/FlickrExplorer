namespace FlickrExplorer.Classes
{
    /// <summary>
    /// Risposta api dettaglio foto
    /// </summary>
    public class GetPhotoInfoResponse : ApiResponse
    {
        public PhotoInfo Photo { get; set; }
    }
}
