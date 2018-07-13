
using Microsoft.EntityFrameworkCore;

namespace Mehrsan.Dal.DB
{
    public partial class WordEntities : DbContext
    {
        public WordEntities(DbContextOptions<WordEntities> options) : base(options)
        {
        }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Word> Words { get; set; }
        
    }
}
