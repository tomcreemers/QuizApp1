using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using PropertyChanged;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class SettingsViewModel : BindableObject
    {
        private string _fullName;
        private string _emailAddress;
        private readonly INavigation _navigation;
        private readonly HomeViewModel _homeViewModel; // Vervanger voor MainPageViewModel

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }

        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                _emailAddress = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand ReturnCommand { get; }

        public SettingsViewModel(INavigation navigation, HomeViewModel homeViewModel)
        {
            _navigation = navigation;
            _homeViewModel = homeViewModel;
            LoadParticipantDetails();

            SaveCommand = new Command(async () => await SaveParticipantDetails());
            ReturnCommand = new Command(async () => await ReturnAsync());
        }

        private async void LoadParticipantDetails()
        {
            var storedEmail = await SecureStorage.GetAsync("email");
            var participant = App.ParticipantRepo?.GetItems()
                .FirstOrDefault(p => p.EmailAddress == storedEmail);

            if (participant != null)
            {
                FullName = participant.FullName;
                EmailAddress = participant.EmailAddress;
            }
        }

        private async Task SaveParticipantDetails()
        {
            // Check of de nieuwe e-mail al in gebruik is door een ander
            var existingParticipant = App.ParticipantRepo?.GetItems()
                .FirstOrDefault(p => p.EmailAddress == EmailAddress 
                                     && p.EmailAddress != (await SecureStorage.GetAsync("email")));

            if (existingParticipant != null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error", 
                    "That email is already registered to another user.", 
                    "OK");
                return;
            }

            var currentParticipant = App.ParticipantRepo?.GetItems()
                .FirstOrDefault(p => p.EmailAddress == (await SecureStorage.GetAsync("email")));

            if (currentParticipant != null)
            {
                currentParticipant.FullName = FullName;
                currentParticipant.EmailAddress = EmailAddress;
                App.ParticipantRepo?.StoreItem(currentParticipant);

                // Sla nieuwe e-mail op in SecureStorage
                await SecureStorage.SetAsync("email", EmailAddress);

                await Application.Current.MainPage.DisplayAlert("Success", "Details updated successfully", "OK");

                // Update de welkomsttekst in HomeViewModel
                _homeViewModel.LoadParticipantName();
            }
        }

        private async Task ReturnAsync()
        {
            await _navigation.PopModalAsync();
        }
    }
}
