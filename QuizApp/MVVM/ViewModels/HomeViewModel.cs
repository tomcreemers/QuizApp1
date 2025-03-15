using System;
using System.Linq;
using System.Windows.Input;
using PropertyChanged;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class HomeViewModel : BindableObject
    {
        private string _greeting;
        private readonly INavigation _navigation;

        public string Greeting
        {
            get => _greeting;
            set
            {
                _greeting = value;
                OnPropertyChanged();
            }
        }

        public ICommand SettingsCommand { get; set; }
        public ICommand FriendsListCommand { get; set; }
        public ICommand QuestionListCommand { get; set; }
        public ICommand DifficultySelectionCommand { get; set; }

        public HomeViewModel(INavigation navigation)
        {
            _navigation = navigation;
            LoadParticipantName();

            SettingsCommand = new Command(NavigateToSettings);
            FriendsListCommand = new Command(NavigateToFriendsList);
            QuestionListCommand = new Command(NavigateToQuestionsList);
            DifficultySelectionCommand = new Command(NavigateToDifficultySelection);
        }

        private void LoadParticipantName()
        {
            try
            {
                var storedEmail = SecureStorage.GetAsync("email").Result;
                var participant = App.ParticipantRepo?.GetItems()
                    .FirstOrDefault(p => p.EmailAddress == storedEmail);

                Greeting = participant != null
                    ? $"Welcome, {participant.FullName}!"
                    : "Welcome!";
            }
            catch (Exception ex)
            {
                Greeting = "Welcome!";
                Application.Current.MainPage.DisplayAlert("Error", $"Error loading participant: {ex.Message}", "OK");
            }
        }

        private void NavigateToSettings()
        {
            var settingsVm = new SettingsViewModel(_navigation, this);
            _navigation.PushModalAsync(new SettingsView(settingsVm));
        }

        private void NavigateToFriendsList()
        {
            _navigation.PushModalAsync(new FriendsListView());
        }

        private void NavigateToQuestionsList()
        {
            _navigation.PushModalAsync(new QuestionsListView());
        }

        private void NavigateToDifficultySelection()
        {
            _navigation.PushModalAsync(new DifficultySelectionView());
        }
    }
}
