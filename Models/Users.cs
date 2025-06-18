using Microsoft.AspNetCore.Identity;

namespace ELibrary.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
