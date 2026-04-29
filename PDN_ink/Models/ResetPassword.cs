using Pdnink.Models;
using System.ComponentModel.DataAnnotations;

namespace Pdnink.Models
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string Token { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}


