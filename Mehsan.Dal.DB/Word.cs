//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mehrsan.Dal.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Word
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Word()
        {
            this.Graphs = new HashSet<Graph>();
            this.Graphs1 = new HashSet<Graph>();
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Graph> Graphs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Graph> Graphs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<History> Histories { get; set; }
        public virtual Language Language { get; set; }
        public virtual Language Language1 { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
