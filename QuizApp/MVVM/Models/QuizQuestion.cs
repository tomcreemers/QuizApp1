using SQLite;
using SQLiteNetExtensions.Attributes;

namespace QuizApp.Models
{
    [Table("QuizQuestionEntity")]
    public class QuizQuestion : TableEntity
    {
        [Column("Prompt"), NotNull]
        public string Prompt { get; set; }

        [Column("QuestionType"), NotNull]
        public string QuestionType { get; set; } // bv. "MultipleChoice" of "TrueFalse"

        [Column("Level"), NotNull]
        public LevelEnum Level { get; set; }

        [ForeignKey(typeof(Topic))]
        public int TopicId { get; set; }

        [ManyToOne, Ignore]
        public Topic? Topic { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All), Ignore]
        public Participant? Creator { get; set; }

        [ForeignKey(typeof(Participant))]
        public int CreatorId { get; set; }
    }
}
