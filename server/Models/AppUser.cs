using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class AppUser:IdentityUser
    {
        [PersonalData]
        [Column(TypeName="nvarchar(500)")]
        public string FullName { get; set; }
      
    }
}
