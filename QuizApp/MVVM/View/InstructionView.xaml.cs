using QuizApp.ViewModels;

namespace QuizApp.Views
{
    public partial class InstructionView : ContentPage
    {
        public InstructionView()
        {
            InitializeComponent();
            BindingContext = new InstructionViewModel(Navigation);
        }
    }
}
