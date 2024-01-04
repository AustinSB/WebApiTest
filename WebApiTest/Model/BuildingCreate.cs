﻿using System.ComponentModel.DataAnnotations;

namespace WebApiTest.Model
{
    public class BuildingCreate
    {
        [Required(ErrorMessage = "You must provide a name.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
