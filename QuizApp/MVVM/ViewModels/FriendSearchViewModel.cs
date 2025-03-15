using System.Windows.Input;
using Microsoft.Maui.Storage;

namespace QuizApp.ViewModels
{
    public class FriendSearchViewModel
    {
        public INavigation? Navigation { get; set; }

        public ICommand CaptureQrCommand { get; set; }

        public FriendSearchViewModel(INavigation? navigation)
        {
            Navigation = navigation;

            CaptureQrCommand = new Command(async () =>
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null)
                {
                    // Verwerk de gemaakte foto
                    var stream = await result.OpenReadAsync();
                    // Hier zou je de QR-code kunnen scannen
                }
            });
        }
    }
}
