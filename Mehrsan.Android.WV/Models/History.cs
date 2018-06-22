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
    [Table("Test")]
    public  class Test
    {
        public string C { get; set; }


    }

    public partial class History
    {
        public bool CreatedInMobile { get; set; }

        //[Column("HistoryId")]
        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int HistoryId { get; set; }
        public System.DateTime ReviewTime { get; set; }
        public int ReviewPeriod { get; set; }
        public bool Result { get; set; }
        public long WordId { get; set; }
        public long ReviewTimeSpan { get; set; }
        public string UpdatedWord { get; set; }
        public string UpdatedMeaning { get; set; }

    }
}