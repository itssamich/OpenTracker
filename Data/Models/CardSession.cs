using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenTracker.Data.Models
{
    [PrimaryKey(nameof(CardId), nameof(SessionId))]
    public class CardSession
    {
        [ForeignKey(nameof(Card))]
        public required int CardId { get; set; }

        public Card? Card { get; set; }
        public required int CardQuantity { get; set; } = 1;

        [ForeignKey(nameof(Session))]
        public required Guid SessionId { get; set; }

        public Session? Session { get; set; }
    }
}
