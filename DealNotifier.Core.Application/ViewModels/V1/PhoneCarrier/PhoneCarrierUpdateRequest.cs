﻿using Catalog.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.PhoneCarrier
{
    public class PhoneCarrierUpdateRequest : IHasId<int>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(3)]
        public string ShortName { get; set; }
    }
}