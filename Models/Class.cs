using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }  // Nullable olarak işaretlendi

        [Required]
        public int PersonCount { get; set; }

        public string? Description { get; set; }  // Nullable olarak işaretlendi

        [Required]
        public bool IsActive { get; set; }  // nullable yap

    }   
}
