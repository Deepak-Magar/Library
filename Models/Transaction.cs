using System.ComponentModel.DataAnnotations;

namespace ELibrary.Models
{
    public class Transaction : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Book selection is required.")]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Member selection is required.")]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Issued date is required.")]
        [Display(Name = "Date Borrowed")]
        [DataType(DataType.Date)]
        public DateTime IssuedDate { get; set; }

        [Display(Name = "Date Returned")]
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }


        public string? BookTitle { get; set; }
        public string? MemberName { get; set; }


        public Book? Book { get; set; }
        public Member? Member { get; set; }

        public bool IsDeleted { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReturnDate <= IssuedDate)
            {
                yield return new ValidationResult(
                    "Return date must be later than the issued date.",
                    new[] { nameof(ReturnDate) });
            }
        }
    }
}
