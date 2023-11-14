﻿using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Level is required")]
        public string Level { get; set; } = string.Empty;
    }
}