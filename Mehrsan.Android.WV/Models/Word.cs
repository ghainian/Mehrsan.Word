using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace Mehrsan.Android.WV.Models
{

    [Table("Word")]
    public class Word
    {
        

        [Column("WordId"),PrimaryKey,NotNull,AutoIncrement]
        public int WordId { get; set; }
        [Column("TargetWord")]
        public string TargetWord { get; set; }
        [Column("Meaning")]
        public string Meaning { get; set; }
        [Column("TargetLanguageId")]
        public long TargetLanguageId { get; set; }
        [Column("MeaningLanguageId")]
        public long MeaningLanguageId { get; set; }
        [Column("NextReviewDate")]
        public System.DateTime NextReviewDate { get; set; }
        [Column("hasOrdNetMeaning")]
        public Nullable<bool> hasOrdNetMeaning { get; set; }
        [Column("hasSound")]
        public Nullable<bool> hasSound { get; set; }
        [Column("IsAmbiguous")]
        public Nullable<bool> IsAmbiguous { get; set; }
        [Column("NofSpace")]
        public Nullable<short> NofSpace { get; set; }
        [Column("WrittenByMe")]
        public Nullable<bool> WrittenByMe { get; set; }
        [Column("IsMovieSubtitle")]
        public Nullable<bool> IsMovieSubtitle { get; set; }
        [Column("StartTime")]
        public Nullable<System.TimeSpan> StartTime { get; set; }
        [Column("EndTime")]
        public Nullable<System.TimeSpan> EndTime { get; set; }
        [Column("MovieName")]
        public string MovieName { get; set; }
        [Column("UserId")]
        public string UserId { get; set; }
        [Column("CreatedInMobile")]
        public bool CreatedInMobile { get; set; }
        [Column("UpdatedInMobile")]
        public bool UpdatedInMobile { get; internal set; }
    }
}