using FlickrExplorer.Models;

namespace FlickrExplorer;

public partial class PhotoDetailPage : ContentPage
{
    public PhotoDetailModel Model { get; set; }
    public PhotoDetailPage(string photoId)
    {
        InitializeComponent();
        Model = new PhotoDetailModel(photoId);
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Task.Run(Model.GetPhotoInfo);
    }
    protected override void OnDisappearing()
    {
        Model.Dispose();
        base.OnDisappearing();

    }

    /// <summary>
    /// Mostra galleria dell'utente
    /// </summary>
    private async void ShowUserGallery(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new UserGalleryPage(e.Parameter as User), true);
    }

    private async void Back(object sender, EventArgs e)
    {
        await Navigation.PopAsync(true);
    }

    /// <summary>
    /// Scarica la foto e consente all'utente di scegliere dove salvarla
    /// </summary>
    private void SavePhoto(object sender, EventArgs e)
    {
        Model.SavePhoto();
    }
}