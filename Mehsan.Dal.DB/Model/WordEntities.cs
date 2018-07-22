using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace Mehrsan.Dal.DB
{
    public partial class WordEntities : DbContext, IWordEntities
    {

        #region Properties

        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Word> Words { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        public static DbContextOptions<WordEntities> Options { get; private set; }

        #endregion

        #region Methods

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

        #endregion

    }
}
