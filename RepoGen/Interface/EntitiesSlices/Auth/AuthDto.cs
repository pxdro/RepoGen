using System.ComponentModel.DataAnnotations;

namespace RepoGen.Interface.EntitiesSlices.Auth
{
    public class AuthDto
    {
        [MinLength(10, ErrorMessage = "Login must have at least 10 characters")]
        [MaxLength(20, ErrorMessage = "Login must be up to 20 characters long")]
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
