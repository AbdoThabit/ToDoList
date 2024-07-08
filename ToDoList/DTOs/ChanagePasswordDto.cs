using System.ComponentModel.DataAnnotations;

namespace ToDoList.DTOs
{
    public class ChanagePasswordDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword {get; set;}

    }
}
