using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenTracker.Data.Models
{
    public class Session
    {
        [Key]
        public Guid SessionId { get; set; }


        [Precision(5, 2)]
        public decimal TotalPrice { get; set; } = 0;

        [ForeignKey(nameof(ApplicationUser))]
        public required string SessionOwnerId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public required DateTime LastUpdated { get; set; } = DateTime.Now;

        [Required]
        public required DateTime SessionStarted { get; set; } = DateTime.Now;

        public DateTime? SessionClosed { get; set; }

        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
