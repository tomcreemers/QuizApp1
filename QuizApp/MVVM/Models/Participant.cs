using SQLite;
using SQLiteNetExtensions.Attributes;

namespace QuizApp.Models
{
    [Table("ParticipantEntity")]
    public class Participant : TableEntity
    {
        [Column("FullName"), Indexed, NotNull]
        public string FullName { get; set; }

        public string Secret { get; set; }

        public string EmailAddress { get; set; }

        public string QrIdentifier { get; set; }

        public int FriendKey { get; set; }

        [Ignore]
        public List<Participant> Connections { get; set; }
    }
}
