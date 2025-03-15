using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows.Input;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class DifficultySelectionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TopicEnum> Topics { get; set; }

        private TopicEnum _selectedTopic;
        private int _selectedLevel = 1; // Default
        private string _participantName;

        public TopicEnum SelectedTopic
        {
            get => _selectedTopic;
            set
            {
                _selectedTopic = value;
                OnPropertyChanged();
            }
        }

        public int SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                _selectedLevel = value;
                OnPropertyChanged();
            }
        }

        public string ParticipantName
        {
            get => _participantName;
            set
            {
                _participantName = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartGameCommand { get; }

        public DifficultySelectionViewModel()
        {
            Topics = new ObservableCollection<TopicEnum>(System.Enum.GetValues(typeof(TopicEnum)).Cast<TopicEnum>());
            LoadParticipantName();
            StartGameCommand = new Command(BeginGameplay);
        }

        private void LoadParticipantName()
        {
            try
            {
                var storedEmail = SecureStorage.GetAsync("email").Result;
                var participant = App.ParticipantRepo?.GetItems()
                    .FirstOrDefault(p => p.EmailAddress == storedEmail);

                ParticipantName = participant != null ? participant.FullName : "Player";
            }
            catch (Exception ex)
            {
                ParticipantName = "Player";
                Application.Current.MainPage.DisplayAlert("Error", $"Error loading participant: {ex.Message}", "OK");
            }
        }

        private async void BeginGameplay()
        {
            if (SelectedTopic == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a topic.", "OK");
                return;
            }

            // Navigeer naar de Gameplay-pagina, geef topic & level door
            await Application.Current.MainPage.Navigation
                .PushModalAsync(new GameplayView(SelectedTopic, (LevelEnum)SelectedLevel));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
