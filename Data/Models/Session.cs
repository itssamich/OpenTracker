using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace OpenTracker.Data.Models
{
    public class Session
    {
        [Key]
        public Guid SessionId { get; set; }

        [Required]
        public required string SessionName { get; set; }

        [Precision(8, 2)]
        public decimal TotalPrice { get; set; } = 0;

        [MaxLength(3)]
        public string? SavedSessionSet { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public required string SessionOwnerId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        [Required]
        public required DateTime LastUpdated { get; set; } = DateTime.Now;

        [Required]
        public required DateTime SessionStarted { get; set; } = DateTime.Now;

        public DateTime? SessionClosed { get; set; }



    }
}
