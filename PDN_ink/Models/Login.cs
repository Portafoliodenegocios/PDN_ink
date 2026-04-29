using System.ComponentModel.DataAnnotations;

namespace Pdnink_Coremvc.Models
{
    public class Login
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string TokenKey { get; set; }

        [Required]
        public Guid AppId { get; set; }

        [Required]
        public string Sin { get; set; }

        public string Origin { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public string Code { get; set; }

        public Guid UserId { get; set; }
        public List<Guid> Lstapps { get; set; }
        public string email { get; set; }

    }
}
