using QuizApp.ViewModels;

namespace QuizApp.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation);
        }

        protected override bool OnBackButtonPressed()
        {
            // Blokkeer terugnavigatie naar de InstructionView
            return true;
        }
    }
}
