using Microsoft.EntityFrameworkCore;

namespace belt.Models{
    public class beltContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public beltContext(DbContextOptions<beltContext> options) : base(options) { }

        public DbSet<users> users{get;set;}

        public DbSet<activity> activity{get;set;}
        public DbSet<participants> participants{get;set;}

        
    }
}