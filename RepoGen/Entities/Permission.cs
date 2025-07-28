using System.ComponentModel.DataAnnotations;

namespace RepoGen.Entities
{
    public class Permission : Entity
    {
        [MaxLength(100, ErrorMessage = "Report name must be up to 100 characters long")]
        [Required(ErrorMessage = "Report name is required")]
        public string Name { get; set; }

        /* EF Core Relation */
        public IEnumerable<User>? Users { get; set; }
    }
}
