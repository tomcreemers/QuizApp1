using System.Collections.ObjectModel;
using System.Windows.Input;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class ManageQuestionsViewModel : BindableObject
    {
        private ObservableCollection<QuizQuestion> _participantQuestions;
        private QuizQuestion _currentQuestion;
        private readonly INavigation _navigation;

        public ObservableCollection<QuizQuestion> ParticipantQuestions
        {
            get => _participantQuestions;
            set
            {
                _participantQuestions = value;
                OnPropertyChanged();
            }
        }

        public QuizQuestion CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCurrentQuestionCommand { get; }
        public ICommand RemoveQuestionCommand { get; }
        public ICommand GoBackCommand { get; }

        public ManageQuestionsViewModel(INavigation navigation)
        {
            _navigation = navigation;
            LoadParticipantQuestions();

            GoBackCommand = new Command(async () =>
            {
                await _navigation.PushModalAsync(new QuestionsListView());
            });

            SaveCurrentQuestionCommand = new Command(async () => await SaveCurrentQuestion());
            RemoveQuestionCommand = new Command<QuizQuestion>(async (question) => await RemoveQuestion(question));
        }

        private void LoadParticipantQuestions()
        {
            var storedEmail = SecureStorage.GetAsync("email").Result;
            var participant = App.ParticipantRepo?.GetItems()
                .FirstOrDefault(p => p.EmailAddress == storedEmail);

            if (participant != null)
            {
                ParticipantQuestions = new ObservableCollection<QuizQuestion>(
                    App.QuizQuestionRepo?.GetItems()
                    .Where(q => q.CreatorId == participant.Id)
                );
            }
        }

        private async Task SaveCurrentQuestion()
        {
            if (CurrentQuestion != null)
            {
                App.QuizQuestionRepo?.StoreItem(CurrentQuestion);
                await Application.Current.MainPage.DisplayAlert("Success", "Question updated successfully", "OK");
                LoadParticipantQuestions();
            }
        }

        private async Task RemoveQuestion(QuizQuestion question)
        {
            if (question != null)
            {
                App.QuizQuestionRepo?.RemoveItem(question);
                LoadParticipantQuestions();
                await Application.Current.MainPage.DisplayAlert("Success", "Question removed", "OK");
            }
        }
    }
}
