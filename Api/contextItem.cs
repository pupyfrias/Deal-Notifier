using Api.Dtos;
using Microsoft.EntityFrameworkCore;


namespace Api
{
    public class contextItem : DbContext
    {
        public contextItem()
        {
        }

        public contextItem(DbContextOptions<contextItem> options) : base(options)
        {
        }
        public DbSet<Item> Item { get; set; }
        public DbSet<User> _User { get; set; }
    }
}
