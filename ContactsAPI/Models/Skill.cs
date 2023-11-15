using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContactsAPI.Models
{
    public class Skill
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Level is required")]
        public string Level { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
