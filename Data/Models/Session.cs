using System.ComponentModel.DataAnnotations;

namespace OpenTracker.Data.Models
{
    public class Session
    {
        [Key]
        public Guid SessionId { get; set; }
    }
}
