using QuizApp.ViewModels;

namespace QuizApp.Views
{
    public partial class DifficultySelectionView : ContentPage
    {
        public DifficultySelectionView()
        {
            InitializeComponent();
            BindingContext = new DifficultySelectionViewModel();
        }

        private async void OnReturnClicked(object sender, EventArgs e)
        {
            // Ga terug naar je HomeView (voorheen MainPage)
            await Navigation.PushModalAsync(new HomeView());
        }

        private void OnBeginClicked(object sender, EventArgs e)
        {
            // Start de game via het ViewModel
            if (BindingContext is DifficultySelectionViewModel vm)
            {
                vm.StartGameCommand.Execute(null);
            }
        }
    }
}
