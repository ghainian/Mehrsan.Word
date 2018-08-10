using Microsoft.EntityFrameworkCore;
using System;

namespace Mehrsan.Dal.DB
{
    public interface IWordEntities:IDisposable
    {
        DbSet<History> Histories { get; set; }
        DbSet<Word> Words { get; set; }
        DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        DbSet<AspNetUser> AspNetUsers { get; set; }
        bool IsDisposed { get; }
    }
}