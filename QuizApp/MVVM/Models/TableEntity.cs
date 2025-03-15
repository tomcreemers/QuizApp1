using SQLite;

namespace QuizApp.Models
{
    public class TableEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
