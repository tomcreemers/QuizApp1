using SQLite;

namespace QuizApp
{
    public class AppConstants
    {
        // Simpele endpoint van de Official Joke API als voorbeeld
        public const string JOKE_ENDPOINT = "https://official-joke-api.appspot.com/jokes/random";

        public const string DBFilename = "QuizApp.db3";

        public const SQLiteOpenFlags Flags =
            // open de database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // maak de database aan als deze niet bestaat
            SQLiteOpenFlags.Create |
            // schakel multi-threaded access in
            SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                return Path.Combine(FileSystem.AppDataDirectory, DBFilename);
            }
        }
    }
}
