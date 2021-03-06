﻿using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace Mehrsan.Dal.DB
{
    public  class WordEntities : DbContext, IWordEntities
    {

        #region Properties

        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Word> Words { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        public static DbContextOptions<WordEntities> Options { get; private set; }
        public bool IsDisposed { get; private set; }

        #endregion

        #region Methods

        public WordEntities(DbContextOptions<WordEntities> options) : base(options)
        {
            
            Options = options;
            IsDisposed = false;
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            this.IsDisposed = true;
        }

        #endregion

    }
}
