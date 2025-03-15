using QuizApp.Models;
using QuizApp.ViewModels;

namespace QuizApp.Views
{
    public partial class GameplayView : ContentPage
    {
        public GameplayView(TopicEnum selectedTopic, LevelEnum selectedLevel)
        {
            InitializeComponent();

            var viewModel = new GameplayViewModel(selectedTopic, selectedLevel)
            {
                Navigation = this.Navigation
            };
            BindingContext = viewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            // Blokkeer terugnavigatie, net als in het origineel
            return true;
        }

        private async void OnAnswerClicked(object sender, EventArgs e)
        {
            // Ga naar de "next round" of iets dergelijks
            await Navigation.PushModalAsync(new NextRoundView());
        }

        private async void OnConsequenceClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NextRoundView());
        }

        private async void OnNextRoundClicked(object sender, EventArgs e)
        {
            // Zelfde logica, of iets anders
            await Navigation.PushModalAsync(new NextRoundView());
        }
    }
}
