using System;
using System.ComponentModel.DataAnnotations;

namespace WebScraping.Core.Domain.Entities
{
    public class Condition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }

    }
}
