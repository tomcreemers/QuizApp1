using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class GameplayViewModel : INotifyPropertyChanged
    {
        private int _currentQuestionIndex;
        private ObservableCollection<QuizQuestion> _questions;
        private int _points;
        private const int WinThreshold = 1; // Stel in naar wens
        private bool _isInteractionEnabled = true;

        private QuizQuestion _currentQuestion;
        public QuizQuestion CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }

        public string ParticipantName { get; set; }
        public INavigation Navigation { get; set; }

        public ICommand SubmitAnswerCommand { get; }
        public ICommand TakeConsequenceCommand { get; }
        public ICommand NextQuestionCommand { get; }

        public bool IsInteractionEnabled
        {
            get => _isInteractionEnabled;
            set
            {
                _isInteractionEnabled = value;
                OnPropertyChanged();
            }
        }

        public int Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged();
                EvaluateWinCondition();
            }
        }

        public GameplayViewModel(TopicEnum selectedTopic, LevelEnum selectedLevel)
        {
            LoadParticipantName();
            
            // Voorbeeld: als je een data-initializer hebt die vragen genereert
            _questions = new ObservableCollection<QuizQuestion>(
                QuizInitializer.GenerateQuestions(selectedTopic, selectedLevel)
            );

            _currentQuestionIndex = 0;
            CurrentQuestion = _questions[_currentQuestionIndex];

            SubmitAnswerCommand = new Command(SubmitAnswer, () => IsInteractionEnabled);
            TakeConsequenceCommand = new Command(TakeConsequence);
            NextQuestionCommand = new Command(MoveToNextQuestion);
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

        private async void SubmitAnswer()
        {
            IsInteractionEnabled = false;
            Points++;
            if (Navigation != null)
            {
                await Navigation.PushModalAsync(new NextRoundView());
            }
        }

        private async void TakeConsequence()
        {
            if (Navigation != null)
            {
                await Navigation.PushModalAsync(new NextRoundView());
            }
        }

        private async void EvaluateWinCondition()
        {
            if (Points >= WinThreshold)
            {
                // Navigate to VictoryView
                if (Navigation != null)
                {
                    await Navigation.PushModalAsync(new VictoryView(Points));
                }
            }
        }

        public void MoveToNextQuestion()
        {
            _currentQuestionIndex = (_currentQuestionIndex + 1) % _questions.Count;
            CurrentQuestion = _questions[_currentQuestionIndex];
            OnPropertyChanged(nameof(CurrentQuestion));
            IsInteractionEnabled = true; // Reset om weer te kunnen antwoorden
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
