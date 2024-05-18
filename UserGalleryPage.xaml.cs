using FlickrExplorer.Models;

namespace FlickrExplorer;

public partial class UserGalleryPage : ContentPage
{
    public double PhotoHeight => DeviceDisplay.MainDisplayInfo.Width / 6d;
    public UserGalleryModel Model { get; set; }
    public UserGalleryPage(User user)
    {
        InitializeComponent();
        Model = new UserGalleryModel(user);
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Task.Run(Model.GetUserGallery);
    }

    protected override void OnDisappearing()
    {
        Model.Dispose();
        base.OnDisappearing();
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
        string photoId = (string)e.Parameter;
        await Navigation.PushAsync(new PhotoDetailPage(photoId), true);
    }

    private async void Back(object sender, EventArgs e)
    {
        await Navigation.PopAsync(true);
    }
}