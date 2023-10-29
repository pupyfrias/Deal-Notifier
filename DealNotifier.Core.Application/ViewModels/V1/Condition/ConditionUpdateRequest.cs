﻿using Catalog.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.Condition
{
    public class ConditionUpdateRequest : IHasId<int>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}