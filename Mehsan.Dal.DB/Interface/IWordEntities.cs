using Microsoft.EntityFrameworkCore;
using System;

namespace Mehrsan.Dal.DB
{
    public interface IWordEntities:IDisposable
    {
        DbSet<History> Histories { get; set; }
        DbSet<Word> Words { get; set; }
        

    }
}