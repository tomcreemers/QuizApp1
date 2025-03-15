using SQLite;
using SQLiteNetExtensions.Attributes;

namespace QuizApp.Models
{
    [Table("TopicEntity")]
    public class Topic : TableEntity
    {
        [Column("TopicName"), Indexed, NotNull]
        public TopicEnum TopicName { get; set; }

        [Column("Level")]
        public LevelEnum Level { get; set; }

        [Ignore]
        public List<QuizQuestion>? RelatedQuestions { get; set; }
    }
}
