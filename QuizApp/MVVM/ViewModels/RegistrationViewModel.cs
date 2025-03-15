using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class RegistrationViewModel
    {
        public INavigation Navigation { get; set; }

        public ICommand RegisterParticipantCommand { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Secret { get; set; }
        public string ConfirmSecret { get; set; }

        public RegistrationViewModel(INavigation navigation)
        {
            Navigation = navigation;

            RegisterParticipantCommand = new Command(async () =>
            {
                if (string.IsNullOrEmpty(FullName) || 
                    string.IsNullOrEmpty(EmailAddress) || 
                    string.IsNullOrEmpty(Secret) || 
                    string.IsNullOrEmpty(ConfirmSecret))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "All fields are required", "OK");
                    return;
                }

                if (!Secret.Equals(ConfirmSecret))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match", "OK");
                    return;
                }

                var existingParticipant = App.ParticipantRepo?.GetItems()
                    .FirstOrDefault(p => p.EmailAddress == EmailAddress);

                if (existingParticipant != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Email is already registered", "OK");
                    return;
                }

                var newParticipant = new Participant
                {
                    FullName = FullName,
                    EmailAddress = EmailAddress,
                    Secret = Secret // In een echte app zou je dit hashen
                };

                App.ParticipantRepo?.StoreItem(newParticipant);
                await StoreLoginDetails(EmailAddress, Secret);

                await Application.Current.MainPage.DisplayAlert("Success", "Registration successful", "OK");

                // Na registratie door naar 'RegisteredView' (voorheen GeregistreerdPage)
                await Navigation.PushModalAsync(new RegisteredView());
            });
        }

        private async Task StoreLoginDetails(string email, string password)
        {
            try
            {
                await SecureStorage.SetAsync("email", email);
                await SecureStorage.SetAsync("password", password);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error storing credentials: {ex.Message}", "OK");
            }
        }
    }
}
