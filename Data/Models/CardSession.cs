using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenTracker.Data.Models
{
    //[PrimaryKey(nameof(CardId), nameof(SessionId))]
    public class CardSession
    {
        [Key]
        public int CardSessionId { get; set; }

        [ForeignKey(nameof(Card))]
        public required int CardId { get; set; }

        public Card? Card { get; set; }
        public required int CardQuantity { get; set; } = 1;

        [ForeignKey(nameof(Session))]
        public required Guid SessionId { get; set; }

        public Session? Session { get; set; }

        [Required]
        public required DateTime LastUpdated { get; set; } = DateTime.Now;

        [Required]

        public required DateTime DateCreated { get; set; } = DateTime.Now;

    }
}
