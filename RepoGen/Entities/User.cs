using System.ComponentModel.DataAnnotations;

namespace RepoGen.Entities
{
    public class User : Entity
    {
        [MinLength(10, ErrorMessage = "Username must have at least 10 characters")]
        [MaxLength(150, ErrorMessage = "Username must be up to 150 characters long")]
        [Required(ErrorMessage = "Username is required")]
        public string Name { get; set; }

        [MinLength(10, ErrorMessage = "User login must have at least 10 characters")]
        [MaxLength(20, ErrorMessage = "User login must be up to 20 characters long")]
        [Required(ErrorMessage = "User login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "User password is required")]
        public string HashedPassword { get; set; }

        [Required(ErrorMessage = "It is mandatory to inform whether the user is Admin")]
        public EnumStatus Admin { get; set; } = EnumStatus.Inactive;

        [Required(ErrorMessage = "User status is required")]
        public EnumStatus Status { get; set; } = EnumStatus.Active;


        /* EF Core Relation */
        public ICollection<Permission>? Permissions { get; set; }
    }
}
