using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace QuizApp.ViewModels
{
    public class NextRoundViewModel : INotifyPropertyChanged
    {
        private string _jokeUrl;

        public string JokeUrl
        {
            get => _jokeUrl;
            set
            {
                _jokeUrl = value;
                OnPropertyChanged();
            }
        }

        public ICommand FetchJokeCommand { get; }

        // Eventueel constructor met logic om de command te koppelen
        public NextRoundViewModel()
        {
            // Voorbeeld: 
            // FetchJokeCommand = new Command(FetchJokeCommand_Executed);
        }

        // private void FetchJokeCommand_Executed()
        // {
        //     // Hier zou je bijvoorbeeld een service aanroepen die een random joke ophaalt
        // }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
