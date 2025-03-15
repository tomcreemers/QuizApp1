using System.Windows.Input;

namespace QuizApp.ViewModels
{
    public class RegisteredViewModel
    {
        public INavigation Navigation { get; set; }
        public ICommand ConfirmRegistrationCommand { get; set; }

        public RegisteredViewModel(INavigation navigation)
        {
            Navigation = navigation;

            ConfirmRegistrationCommand = new Command(async () =>
            {
                // Na registratie terug naar de HomeView (voorheen MainPage)
                await Navigation.PushModalAsync(new HomeView());
            });
        }
    }
}
