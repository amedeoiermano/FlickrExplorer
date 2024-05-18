using FlickrExplorer.Classes;

namespace FlickrExplorer
{
    public class ApiClient
    {
        /// <summary>
        /// Chiave api Flickr
        /// </summary>
        private const string API_KEY = "a52af8c295a87f18bfcc4b2e6b15e7ed";
        /// <summary>
        /// Url comprensivo di chiave api e formato di risposta desiderato (JSON)
        /// </summary>
        private const string BASE_URL = $"https://api.flickr.com/services/rest?api_key={API_KEY}&format=json";

        /// <summary>
        /// Metodi invocabili su api Flickr
        /// </summary>
        private class MethodsDefinitions
        {
            /// <summary>
            /// Metodo ricerca foto
            /// </summary>
            public const string SEARCH_PHOTOS = "flickr.photos.search";
            /// <summary>
            /// Metodo recupero dettagli foto
            /// </summary>
            public const string GET_PHOTO_INFO = "flickr.photos.getInfo";
            /// <summary>
            /// Metodo per recupero di foto pubbliche di un utente
            /// </summary>
            public const string GET_USER_PUBLIC_PHOTOS = "flickr.people.getPublicPhotos";
        }

        private static ApiClient _singleton { get; set; }
        /// <summary>
        /// Istanza globale del client
        /// </summary>
        public static ApiClient Instance => _singleton ??= new ApiClient();

        private HttpClient _httpClient;
        private ApiClient()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Restituisce le foto che contengono le parole chiave indicate nel parametro query
        /// </summary>
        /// <param name="query">Testo contenente parole chiave con cui cercare foto</param>
        /// <param name="cts">Token source per cancellazione richiesta asincrona</param>
        /// <param name="page">Pagina corrente risultati</param>
        /// <param name="pageFactor">Numero elementi restituiti per pagina</param>
        /// <returns>Lista foto con stato dell'operazione</returns>
        public async Task<PhotosResponse> SearchPhotos(string query, CancellationTokenSource cts, int page = 0, int pageFactor = 50)
        {
            PhotosResponse resp = new PhotosResponse();
            try
            {
                // costruisce url
                string url = $"{BASE_URL}&method={MethodsDefinitions.SEARCH_PHOTOS}&page={page}&per_page={pageFactor}&text={query}";
                // effettua richiesta
                var res = await _httpClient.GetAsync(url, cts.Token);
                res.EnsureSuccessStatusCode();
                // legge la risposta come testo e la processa convertendola nell'oggetto atteso
                resp = ApiResponse.GetData<PhotosResponse>(await res.Content.ReadAsStringAsync());
            }
            catch
            {
                resp.Status = Status.Fail;
            }

            return resp;
        }

        /// <summary>
        /// Recupera foto pubbliche di uno specifico utente
        /// </summary>
        /// <param name="userId">Id dell'utente</param>
        /// <param name="cts">Token source per cancellazione richiesta asincrona</param>
        /// <param name="page">Pagina corrente risultati</param>
        /// <param name="pageFactor">Numero elementi restituiti per pagina</param>
        /// <returns>ista foto con stato dell'operazione</returns>
        public async Task<PhotosResponse> GetUserPublicPhotos(string userId, CancellationTokenSource cts, int page = 0, int pageFactor = 50)
        {
            PhotosResponse resp = new PhotosResponse();
            try
            {   // costruisce url
                string url = $"{BASE_URL}&method={MethodsDefinitions.GET_USER_PUBLIC_PHOTOS}&page={page}&per_page={pageFactor}&user_id={userId}";
                // effettua richiesta
                var res = await _httpClient.GetAsync(url, cts.Token);
                res.EnsureSuccessStatusCode();
                // legge la risposta come testo e la processa convertendola nell'oggetto atteso
                resp = ApiResponse.GetData<PhotosResponse>(await res.Content.ReadAsStringAsync());
            }
            catch
            {
                resp.Status = Status.Fail;
            }

            return resp;
        }

        /// <summary>
        /// Recupera dettagli di una foto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        public async Task<GetPhotoInfoResponse> GetPhotoInfo(string id, CancellationTokenSource cts)
        {
            GetPhotoInfoResponse resp = new GetPhotoInfoResponse();
            try
            {
                // costruisce url
                string url = $"{BASE_URL}&method={MethodsDefinitions.GET_PHOTO_INFO}&photo_id={id}";
                // effettua richiesta
                var res = await _httpClient.GetAsync(url, cts.Token);
                res.EnsureSuccessStatusCode();
                // legge la risposta come testo e la processa convertendola nell'oggetto atteso
                resp = ApiResponse.GetData<GetPhotoInfoResponse>(await res.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                resp.Status = Status.Fail;
            }

            return resp;
        }

        /// <summary>
        /// Scarica una foto come byte array dal suo url
        /// </summary>
        /// <param name="url">Url della foto da scaricare</param>
        /// <returns></returns>
        public async Task<byte[]> DownloadPhoto(string url)
        {
            try
            {
                return await _httpClient.GetByteArrayAsync(url);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }




}
