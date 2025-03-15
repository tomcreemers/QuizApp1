using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class QuestionListViewModel : BindableObject
    {
        private string _promptText;
        private string _selectedTopicString;
        private LevelEnum _selectedLevel;
        private string _selectedQuestionType;
        private ObservableCollection<QuizQuestion> _participantQuestions;
        private readonly INavigation _navigation;

        public string PromptText
        {
            get => _promptText;
            set
            {
                _promptText = value;
                OnPropertyChanged();
            }
        }

        public string SelectedTopicString
        {
            get => _selectedTopicString;
            set
            {
                _selectedTopicString = value;
                OnPropertyChanged();
            }
        }

        public LevelEnum SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                _selectedLevel = value;
                OnPropertyChanged();
            }
        }

        public string SelectedQuestionType
        {
            get => _selectedQuestionType;
            set
            {
                _selectedQuestionType = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> TopicOptions { get; set; }
        public ObservableCollection<LevelEnum> LevelOptions { get; set; }
        public ObservableCollection<string> QuestionTypes { get; set; }

        public ObservableCollection<QuizQuestion> ParticipantQuestions
        {
            get => _participantQuestions;
            set
            {
                _participantQuestions = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddQuestionCommand { get; }
        public ICommand ReturnCommand { get; }
        public ICommand ManageQuestionsCommand { get; }

        public QuestionListViewModel(INavigation navigation)
        {
            _navigation = navigation;

            ReturnCommand = new Command(() =>
            {
                _navigation.PushModalAsync(new HomeView());
            });

            ManageQuestionsCommand = new Command(() =>
            {
                _navigation.PushModalAsync(new ManageQuestionsView());
                LoadParticipantQuestions();
            });

            TopicOptions = new ObservableCollection<string>
            {
                "Party",
                "Friends",
                "Family"
            };

            LevelOptions = new ObservableCollection<LevelEnum>
            {
                LevelEnum.Easy,
                LevelEnum.Medium,
                LevelEnum.Hard
            };

            // Voorbeeld: "Truth" / "Dare" → "MultipleChoice" / "TrueFalse"?
            QuestionTypes = new ObservableCollection<string>
            {
                "Truth",
                "Dare"
            };

            AddQuestionCommand = new Command(async () => await AddQuestion());
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

        private async Task AddQuestion()
        {
            if (string.IsNullOrWhiteSpace(PromptText))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Question text cannot be empty", "OK");
                return;
            }

            var storedEmail = SecureStorage.GetAsync("email").Result;
            var participant = App.ParticipantRepo?.GetItems()
                .FirstOrDefault(p => p.EmailAddress == storedEmail);

            if (participant == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Participant not found", "OK");
                return;
            }

            // Om TopicEnum te matchen, hier voorbeeld
            if (!Enum.TryParse<TopicEnum>(SelectedTopicString, out var parsedTopic))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Topic not recognized", "OK");
                return;
            }

            // Vind of maak de Topic in de DB
            var topicEntity = App.TopicRepo?.GetItems()
                .FirstOrDefault(t => t.TopicName == parsedTopic);

            if (topicEntity == null)
            {
                // Optioneel: automatisch aanmaken
                topicEntity = new Topic
                {
                    TopicName = parsedTopic,
                    Level = SelectedLevel
                };
                App.TopicRepo?.StoreItem(topicEntity);
            }

            var newQuestion = new QuizQuestion
            {
                Prompt = PromptText,
                QuestionType = SelectedQuestionType,
                Level = SelectedLevel,
                CreatorId = participant.Id,
                TopicId = topicEntity.Id
            };

            App.QuizQuestionRepo?.StoreItem(newQuestion);

            await Application.Current.MainPage.DisplayAlert("Success", "Question added successfully", "OK");
        }
    }
}
