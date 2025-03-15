using System.Linq;
using System.Windows.Input;
using PropertyChanged;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class LoginViewModel
    {
        public List<Participant> Participants { get; set; }
        public INavigation Navigation { get; set; }

        public ICommand SignInCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand NavigateToRegistrationCommand { get; set; }

        public string EmailAddress { get; set; }
        public string Secret { get; set; }

        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;

            SignInCommand = new Command(async () =>
            {
                if (string.IsNullOrEmpty(EmailAddress) || string.IsNullOrEmpty(Secret))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "All fields are required", "OK");
                    return;
                }

                var participant = App.ParticipantRepo?.GetItems()
                    .FirstOrDefault(p => p.EmailAddress == EmailAddress);

                if (participant == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid email", "Try Again");
                    return;
                }

                if (!participant.Secret.Equals(Secret, StringComparison.Ordinal))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid password", "Try Again");
                    return;
                }

                await Application.Current.MainPage.DisplayAlert("Success", "You are logged in!", "OK");
                StoreCredentials(EmailAddress, Secret);
                await Navigation.PushModalAsync(new HomeView());
            });

            GoBackCommand = new Command(async () =>
            {
                await Navigation.PushModalAsync(new HomeView());
            });

            NavigateToRegistrationCommand = new Command(async () =>
            {
                await Navigation.PushModalAsync(new RegistrationView());
            });
        }

        private void StoreCredentials(string email, string password)
        {
            try
            {
                SecureStorage.SetAsync("email", email);
                SecureStorage.SetAsync("password", password);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", $"Error storing credentials: {ex.Message}", "OK");
            }
        }

        public void AttemptAutoLogin()
        {
            try
            {
                var storedEmail = SecureStorage.GetAsync("email").Result;
                var storedSecret = SecureStorage.GetAsync("password").Result;

                if (!string.IsNullOrEmpty(storedEmail) && !string.IsNullOrEmpty(storedSecret))
                {
                    var participant = App.ParticipantRepo?.GetItems()
                        .FirstOrDefault(p => p.EmailAddress == storedEmail && p.Secret == storedSecret);

                    if (participant != null)
                    {
                        Navigation.PushModalAsync(new HomeView());
                    }
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", $"Error loading credentials: {ex.Message}", "OK");
            }
        }
    }
}
