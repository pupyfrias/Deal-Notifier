
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Item
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public decimal? price { get; set; }
        public decimal? old_price { get; set; }
        public decimal? saving { get; set; }
        public string? link { get; set; }
        public int? condition { get; set; }
        public int? status { get; set; }
        public int shop { get; set; }
        public string? image { get; set; }
        public DateTime? date_price_change { get; set; }
        public decimal? saving_percent { get; set; }
        public int? type { get; set; }




    }
}
