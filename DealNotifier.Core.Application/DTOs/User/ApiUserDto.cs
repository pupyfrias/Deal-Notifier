﻿using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.DTOs.User
{
    public class ApiUserDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}