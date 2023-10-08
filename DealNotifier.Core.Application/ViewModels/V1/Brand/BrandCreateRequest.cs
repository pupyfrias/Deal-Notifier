﻿using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.Brand
{
    public class BrandCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}