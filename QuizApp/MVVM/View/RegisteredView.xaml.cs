using QuizApp.ViewModels;

namespace QuizApp.Views
{
    public partial class RegisteredView : ContentPage
    {
        public RegisteredView()
        {
            InitializeComponent();
            BindingContext = new RegisteredViewModel(Navigation);
        }

        private async void OnContinueClicked(object sender, EventArgs e)
        {
            // Eventueel direct terug naar HomeView of in je VM
            await Navigation.PushModalAsync(new HomeView());
        }
    }
}
