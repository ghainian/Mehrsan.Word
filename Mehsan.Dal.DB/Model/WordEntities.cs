
using Microsoft.EntityFrameworkCore;

namespace Mehrsan.Dal.DB
{
    public partial class WordEntities : DbContext
    {


        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Word   > Words { get; set; }
        public static DbContextOptions<WordEntities> Options { get; private set; }

        public WordEntities(DbContextOptions<WordEntities> options) : base(options)
        {
            Options = options;
        }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        
    }
}
