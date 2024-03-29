﻿using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.User
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your password id limited to {2} to {1}", MinimumLength = 6)]
        public string Password { get; set; }
    }
}