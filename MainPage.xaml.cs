using FlickrExplorer.Models;

namespace FlickrExplorer
{
    public partial class MainPage : ContentPage
    {
        public SearchPhotosModel Model { get; set; } = new SearchPhotosModel();

        /// <summary>
        /// Fattore di dimensione celle griglia risultati di ricerca
        /// </summary>
        public double PhotoHeight => DeviceDisplay.MainDisplayInfo.Width / 6d;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void PhotoList_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            // quando l'ultimo elemento diventa visibile, prosegue con la paginazione dei risultati
            if (e.LastVisibleItemIndex == Model.Photos.Count - 1)
                await Model.UpdateResults();
        }

        /// <summary>
        /// Mostra dettaglio foto
        /// </summary>
        private async void OpenPhoto(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new PhotoDetailPage((string)e.Parameter), true);
        }
    }

}
