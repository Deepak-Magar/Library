using System;
using System.ComponentModel.DataAnnotations;

namespace ELibrary.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Join Date is required.")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        public bool isDeleted { get; set; }



    }
}
