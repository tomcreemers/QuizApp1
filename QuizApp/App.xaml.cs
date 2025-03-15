using QuizApp.Models;
using QuizApp.Repositories;
using Microsoft.Maui.Controls;

namespace QuizApp;

public partial class App : Application
{
    // Publieke statische repositories (vergelijkbaar met de oude UserRepo, etc.)
    public static BaseDataRepository<Player>? PlayerRepository { get; private set; }
    public static BaseDataRepository<QuizGame>? GameRepository { get; private set; }
    public static BaseDataRepository<QuizQuestion>? QuestionRepository { get; private set; }
    public static BaseDataRepository<QuizCategory>? CategoryRepository { get; private set; }

    public App(
        BaseDataRepository<Player> playerRepo,
        BaseDataRepository<QuizGame> gameRepo,
        BaseDataRepositorusing QuizApp.Models;
using QuizApp.Repositories;

namespace QuizApp
{
    public partial class App : Application
    {
        // Statische properties voor de repositories
        public static GenericRepository<Participant>? ParticipantRepo { get; private set; }
        public static GenericRepository<Session>? SessionRepo { get; private set; }
        public static GenericRepository<QuizQuestion>? QuizQuestionRepo { get; private set; }
        public static GenericRepository<Topic>? TopicRepo { get; private set; }

        public App(
            GenericRepository<Participant> participantRepo,
            GenericRepository<Session> sessionRepo,
            GenericRepository<QuizQuestion> quizQuestionRepo,
            GenericRepository<Topic> topicRepo)
        {
            InitializeComponent();

            ParticipantRepo = participantRepo;
            SessionRepo = sessionRepo;
            QuizQuestionRepo = quizQuestionRepo;
            TopicRepo = topicRepo;

            MainPage = new ContentPage
            {
                Content = new Label
                {
                    Text = "Welcome to QuizApp!",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };
        }

        protected override async void OnStart()
        {
            base.OnStart();
            // Als je permissies nodig hebt, kun je dat hier doen:
            // await RequestStoragePermission();
        }

        private async Task RequestStoragePermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }

            if (status != PermissionStatus.Granted)
            {
                if (MainPage != null)
                {
                    await MainPage.DisplayAlert("Permission Denied", 
                        "Storage permission is required to proceed.", 
                        "OK");
                }
            }
        }
    }
}
y<QuizQuestion> questionRepo,
        BaseDataRepository<QuizCategory> categoryRepo)
    {
        InitializeComponent();

       
        DependencyService.Register<ISimpleApiService, SimpleApiService>();

        PlayerRepository = playerRepo;
        GameRepository = gameRepo;
        QuestionRepository = questionRepo;
        CategoryRepository = categoryRepo;

        MainPage = new InstructionPage(); 
    }

    protected override async void OnStart()
    {
        base.OnStart();
    }

    private async Task RequestWritePermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.StorageWrite>();
        }

        if (status != PermissionStatus.Granted && MainPage != null)
        {
            await MainPage.DisplayAlert("Permission Denied",
                "Write permission is required to store local data.",
                "OK");
        }
    }
}
