using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace FlickrExplorer.Models
{
    public class SearchPhotosModel : ObservableObject, IDisposable
    {
        private string _query;
        private bool _noResults;
        private bool _searchInProgress;
        private CancellationTokenSource _searchCancellationTokenSource, _updateCancellationTokenSource;
        private System.Timers.Timer _searchTimer;
        /// <summary>
        /// Testo inserito dall'utente con cui effettuare la ricerca
        /// </summary>
        public string Query
        {
            get => _query;
            set
            {
                // ricerca resettata. Resetta flag nessun risultato e timer della barra di ricerca
                if (string.IsNullOrEmpty(value))
                {
                    _searchTimer.Stop();
                    Photos.Clear();
                    NoResults = false;
                }
                else
                {
                    // annulla la ricerca in corso
                    _searchCancellationTokenSource?.Cancel();

                    // reset del timer
                    _searchTimer.Stop();
                    _searchTimer.Start();
                }
                SetProperty(ref _query, value);
            }
        }
        /// <summary>
        /// Indica se la ricerca non ha restituito risultati
        /// </summary>
        public bool NoResults { get => _noResults; set => SetProperty(ref _noResults, value); }
        /// <summary>
        /// Pagina corrente risultati
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Pagine totali risultati
        /// </summary>
        public int Pages { get; set; }
        /// <summary>
        /// Numero di foto per pagina mostrate
        /// </summary>
        public int PerPage { get; set; }
        /// <summary>
        /// Totale delle foto restituite
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Lista delle foto
        /// </summary>
        public ObservableCollection<Photo> Photos { get; set; } = new ObservableCollection<Photo>();
        /// <summary>
        /// Indica se c'è già una ricerca in corso
        /// </summary>
        public bool SearchInProgress { get => _searchInProgress; set => SetProperty(ref _searchInProgress, value); }

        public SearchPhotosModel()
        {
            _searchTimer = new System.Timers.Timer(300);
            _searchTimer.Elapsed += OnSearchTimerElapsed;
            _searchTimer.AutoReset = false;
        }
        public void Dispose()
        {
            _searchTimer.Stop();
            _searchTimer.Elapsed -= OnSearchTimerElapsed;
            _searchCancellationTokenSource?.Cancel();
            _updateCancellationTokenSource?.Cancel();
        }

        private async void OnSearchTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // crea un nuovo CancellationTokenSource
            _searchCancellationTokenSource = new CancellationTokenSource();

            // se è già in corso una ricerca, non procede
            if (SearchInProgress)
                return;

            // imposta flag per feedback UI
            NoResults = false;
            SearchInProgress = true;

            try
            {
                // effettua la ricerca
                var resp = await ApiClient.Instance.SearchPhotos(Query, cts: _searchCancellationTokenSource);

                if (resp.Status == Status.Ok)
                {
                    Page = resp.Photos.Page;
                    Pages = resp.Photos.Pages;
                    PerPage = resp.Photos.PerPage;
                    Total = resp.Photos.Total;
                    NoResults = Total == 0;

                    Photos.Clear();
                    foreach (var item in resp.Photos.Photo)
                        Photos.Add(item);
                }
            }
            catch (OperationCanceledException)
            {
                // la ricerca è stata annullata
            }
            finally
            {
                SearchInProgress = false;
            }
        }
        internal async Task UpdateResults()
        {
            // verifica se è stata raggiunta l'ultima pagina 
            if (Page + 1 >= Pages)
                return;

            // ricerca in corso
            if (SearchInProgress)
                return;

            SearchInProgress = true;

            _updateCancellationTokenSource = new CancellationTokenSource();

            // continua la ricerca
            var resp = await ApiClient.Instance.SearchPhotos(Query, _updateCancellationTokenSource, page: Page + 1);
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
