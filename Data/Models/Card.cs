using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace OpenTracker.Data.Models
{
    public class Card
    {
        [Key]
        public int CardId {  get ; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string OracleId { get; set; }

        [Required]
        public required string ImageURL { get; set; }

        [Required]
        public required string CardURL { get; set; }

        public double? Price { get; set; }

        [Required]
        public required DateTime LastUpdated { get; set; } = DateTime.Now;

        [Required]

        public required DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
