using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows.Input;

namespace QuizApp.ViewModels
{
    public class VictoryViewModel : INotifyPropertyChanged
    {
        private string _winnerName;
        public string WinnerName
        {
            get => _winnerName;
            set
            {
                _winnerName = value;
                OnPropertyChanged();
            }
        }

        public int Score { get; set; }
        public ICommand ReturnToDifficultyCommand { get; }

        public VictoryViewModel(int score)
        {
            LoadParticipantName();
            Score = score;
            ReturnToDifficultyCommand = new Command(ReturnToDifficultySelection);
        }

        private void LoadParticipantName()
        {
            try
            {
                var storedEmail = SecureStorage.GetAsync("email").Result;
                var participant = App.ParticipantRepo?.GetItems()
                    .FirstOrDefault(p => p.EmailAddress == storedEmail);

                WinnerName = participant != null ? participant.FullName : "Player";
            }
            catch (Exception ex)
            {
                WinnerName = "Player";
                Application.Current.MainPage.DisplayAlert("Error", $"Error loading participant: {ex.Message}", "OK");
            }
        }

        private async void ReturnToDifficultySelection()
        {
            await Application.Current.MainPage.Navigation
                .PushModalAsync(new DifficultySelectionView());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
