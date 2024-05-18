using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace FlickrExplorer.Models
{
    public class UserGalleryModel : ObservableObject, IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource, _cancellationTokenSource2;
        private string _ownerUserNsid;
        private string _ownerIconUrl;
        private string _ownerUsername;
        private bool _searchInProgress;

        public int Page { get; set; }
        public int Pages { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }

        public string OwnerIconUrl
        {
            get => _ownerIconUrl;
            set => SetProperty(ref _ownerIconUrl, value);
        }

        public string OwnerUsername
        {
            get => _ownerUsername;
            set => SetProperty(ref _ownerUsername, value);
        }
        public ObservableCollection<Photo> Photos { get; set; } = new ObservableCollection<Photo>();
        public bool SearchInProgress { get => _searchInProgress; set => SetProperty(ref _searchInProgress, value); }

        public UserGalleryModel(User user)
        {
            _ownerUserNsid = user.Nsid;
            OwnerIconUrl = user.IconUrl;
            OwnerUsername = user.Username;
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource2?.Cancel();
        }

        /// <summary>
        /// Recupera foto pubbliche dell'utente
        /// </summary>
        /// <returns></returns>
        public async Task GetUserGallery()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            // recupero già in corso
            if (SearchInProgress)
                return;

            SearchInProgress = true;

            try
            {
                var resp = await ApiClient.Instance.GetUserPublicPhotos(_ownerUserNsid, cts: _cancellationTokenSource);

                if (resp.Status == Status.Ok)
                {
                    Page = resp.Photos.Page;
                    Pages = resp.Photos.Pages;
                    PerPage = resp.Photos.PerPage;
                    Total = resp.Photos.Total;

                    Photos.Clear();
                    foreach (var item in resp.Photos.Photo)
                        Photos.Add(item);
                }
            }
            catch (OperationCanceledException)
            {
                // la ricerca è stata annullata, nessuna azione necessaria
            }
            finally
            {
                SearchInProgress = false;
            }
        }
        internal async Task UpdateResults()
        {
            // tutti i risultati mostrati
            if (Page + 1 >= Pages)
                return;
            // ricerca in corso
            if (SearchInProgress) return;
            SearchInProgress = true;
            _cancellationTokenSource2 = new CancellationTokenSource();
            var resp = await ApiClient.Instance.GetUserPublicPhotos(_ownerUserNsid, _cancellationTokenSource2, page: Page + 1);
            if (resp.Status == Status.Ok)
            {
                Page = resp.Photos.Page;
                Pages = resp.Photos.Pages;
                PerPage = resp.Photos.PerPage;
                Total = resp.Photos.Total;
                foreach (var item in resp.Photos.Photo)
                    Photos.Add(item);
            }
            SearchInProgress = false;
        }
    }
}
