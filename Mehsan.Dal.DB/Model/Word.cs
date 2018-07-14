
namespace Mehrsan.Dal.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Word")]
    public partial class Word
    {
        public Word()
        {
            this.Histories = new HashSet<History>();
        }

        public long Id { get; set; }
        public string TargetWord { get; set; }
        public string Meaning { get; set; }
        public long TargetLanguageId { get; set; }
        public long MeaningLanguageId { get; set; }
        public System.DateTime NextReviewDate { get; set; }
        public Nullable<bool> hasOrdNetMeaning { get; set; }
        public Nullable<bool> hasSound { get; set; }
        public Nullable<bool> IsAmbiguous { get; set; }
        public Nullable<short> NofSpace { get; set; }
        public Nullable<bool> WrittenByMe { get; set; }
        public Nullable<bool> IsMovieSubtitle { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public string MovieName { get; set; }
        public string UserId { get; set; }
        
        public virtual ICollection<History> Histories { get; set; }
    }
}
