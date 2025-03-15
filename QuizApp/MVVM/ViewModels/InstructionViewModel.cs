using System.Linq;
using System.Windows.Input;
using QuizApp.Models;
using PropertyChanged;

namespace QuizApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class InstructionViewModel
    {
        public INavigation Navigation { get; set; }

        public ICommand ValidateStoredCredentialsCommand { get; set; }
        public ICommand SignOutCommand { get; set; }

        public InstructionViewModel(INavigation navigation)
        {
            Navigation = navigation;

            ValidateStoredCredentialsCommand = new Command(async () =>
            {
                var storedEmail = await SecureStorage.GetAsync("email");
                var storedSecret = await SecureStorage.GetAsync("password");

                if (!string.IsNullOrEmpty(storedEmail) && !string.IsNullOrEmpty(storedSecret))
                {
                    var participant = App.ParticipantRepo?.GetItems()
                        .FirstOrDefault(p => p.EmailAddress == storedEmail && p.Secret == storedSecret);

                    if (participant != null)
                    {
                        // Ga naar HomeView
                        await Navigation.PushModalAsync(new HomeView());
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
                        await Navigation.PushModalAsync(new LoginView());
                    }
                }
                else
                {
                    // Geen credentials, dus direct naar Login
                    await Navigation.PushModalAsync(new LoginView());
                }
            });

            SignOutCommand = new Command(async () =>
            {
                SecureStorage.Default.RemoveAll();

                await Application.Current.MainPage.DisplayAlert("Success", "Logged out successfully", "OK");
                await Navigation.PushModalAsync(new LoginView());
            });
        }
    }
}
