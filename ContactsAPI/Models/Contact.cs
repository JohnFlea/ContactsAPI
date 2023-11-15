using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ContactsAPI.Models
{
    public class Contact
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lastname is required")]
        [DataMember]
        public string Lastname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fullname is required")]
        [DataMember]
        public string Fullname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [DataMember]
        public string Address { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid pattern.")]
        [DataMember]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Invalid pattern.")]
        [DataMember]
        public string Mobile { get; set; } = string.Empty;

        [SwaggerSchema(ReadOnly = true)]
        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
}
