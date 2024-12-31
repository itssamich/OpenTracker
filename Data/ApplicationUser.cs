using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OpenTracker.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Required]
    public required string FirstName { get; set; }

    [Required] 
    public required string LastName { get; set; }

}

