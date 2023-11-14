using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lastname is required")]
        public string Lastname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fullname is required")]
        public string Fullname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", ErrorMessage = "Invalid pattern.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile is required")]
        public string Mobile { get; set; } = string.Empty;
    }
}
