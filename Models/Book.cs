using System.ComponentModel.DataAnnotations;

namespace ELibrary.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Published Year is required.")]
        [Display(Name = "Published Year")]
        public int Year { get; set; }
        public bool isDeleted { get; set; }

        public bool IsAvailable { get; set; } = true;


    }
}
