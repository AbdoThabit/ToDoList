using System.ComponentModel.DataAnnotations;

namespace ToDoList.DTOs
{
    public class UserRegistrationDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Address { get; set; }

        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
        [Required]
        public string Email { get; set; }

        [RegularExpression(@"^01[0-2]{1}[0-9]{8}$")]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string? Password { get; set; }

        [Compare("Password")]
        [Required]

        public string? ConfirmPassword { get; set; }


    }
}
