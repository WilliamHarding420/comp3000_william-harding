using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SecurityWebSite.DatabaseModels
{
    [PrimaryKey("UserID")]
    public class User
    {

        [Required]
        public int UserID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Salt { get; set; }

    }
}
