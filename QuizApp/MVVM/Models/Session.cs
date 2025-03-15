using SQLite;

namespace QuizApp.Models
{
    [Table("SessionEntity")]
    public class Session : TableEntity
    {
        [Ignore]
        public List<Participant>? Participants { get; set; }

        [Ignore]
        public Topic? SelectedTopic { get; set; }
    }
}
