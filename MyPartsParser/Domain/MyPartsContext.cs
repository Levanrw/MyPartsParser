using Microsoft.EntityFrameworkCore;

namespace MyPartsParser.Domain
{
    public class MyPartsContext: DbContext
    {
        public DbSet<MyPartsUser> MyPartsUsers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost; Initial Catalog=MyAuto; Trusted_Connection=True;Connection Timeout=600");
               // optionsBuilder.UseSqlServer("Data Source=10.64.4.21; Initial Catalog=MyParts; user id=pabuser;password=u6JVj6lAU9nLJh1FaDpr");
            }
        }
    }
}

