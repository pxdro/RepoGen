using System.ComponentModel.DataAnnotations;

namespace RepoGen.Entities
{
    public class LogContent
    {
        [Required]
        public DateTime Data { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(150)]
        public string User { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Report { get; set; } = string.Empty;
    }
}
