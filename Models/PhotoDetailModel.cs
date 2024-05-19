using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using FlickrExplorer.Classes;
using FlickrExplorer.Resources.Strings;
using System.Collections.ObjectModel;

namespace FlickrExplorer.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PhotoDetailModel : ObservableObject, IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource, _savePhotocancellationTokenSource;

        private string _id;
        private string _secret;
        private string _server;
        private User _owner;
        private string _ownerIconUrl;
        private string _ownerUsername;
        private string _url;
        private string _title;
        private string _description;
        private DateTimeOffset _uploadDate;
        private string _photoFileName;
        private bool _photoLoading;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Secret
        {
            get => _secret;
            set => SetProperty(ref _secret, value);
        }

        public string Server
        {
            get => _server;
            set => SetProperty(ref _server, value);
        }

        public User Owner
        {
            get => _owner;
            set => SetProperty(ref _owner, value);
        }


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
        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public DateTimeOffset UploadDate
        {
            get => _uploadDate;
            set => SetProperty(ref _uploadDate, value);
        }
        /// <summary>
        /// Lista tag foto
        /// </summary>
        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();


        public PhotoDetailModel(string id)
        {
            Id = id;
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _savePhotocancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Recupera dettagli foto
        /// </summary>
        /// <returns></returns>
        public async Task<GetPhotoInfoResponse> GetPhotoInfo()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var resp = await ApiClient.Instance.GetPhotoInfo(Id, cts: _cancellationTokenSource);
            if (resp.Status == Status.Ok)
            {
                Id = resp.Photo.Id;
                Secret = resp.Photo.Secret;
                Server = resp.Photo.Server;
                Owner = resp.Photo.Owner;
                OwnerIconUrl = resp.Photo.Owner.IconUrl;
                OwnerUsername = resp.Photo.Owner.Username;
                Title = resp.Photo.Title.Content;
                Description = resp.Photo.Description.Content;
                Url = resp.Photo.Url;
                _photoFileName = resp.Photo.PhotoFileName;
                if (int.TryParse(resp.Photo.DateUploaded, out int unix))
                    UploadDate = DateTimeOffset.FromUnixTimeSeconds(unix);
                if (resp.Photo.Tags != null)
                    foreach (var tag in resp.Photo.Tags.Tag)
                        Tags.Add(tag);
            }

            return resp;
        }

        /// <summary>
        /// Avvia il download della foto e propone il salvataggio all'utente
        /// </summary>
        /// <returns></returns>
        public async Task SavePhoto()
        {
            _savePhotocancellationTokenSource = new CancellationTokenSource();
            bool success = true;
            try
            {
                var photoData = await ApiClient.Instance.DownloadPhoto(Url);
                using (var stream = new MemoryStream(photoData))
                {
                    var fileSaverResult = await FileSaver.Default.SaveAsync(_photoFileName, stream, _savePhotocancellationTokenSource.Token);
                }

            }
            catch
            {
                success = false;
            }

            await Toast.Make(success ? Strings.PhotoSaveSuccessful : Strings.PhotoSaveUnsuccessful, CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
        }
    }
}
