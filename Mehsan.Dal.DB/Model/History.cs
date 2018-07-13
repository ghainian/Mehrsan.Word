using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrsan.Dal.DB
{
    public partial class History
    {
        public long Id { get; set; }
        public System.DateTime ReviewTime { get; set; }
        public int ReviewPeriod { get; set; }
        public bool Result { get; set; }
        public long WordId { get; set; }
        public long ReviewTimeSpan { get; set; }
        public string UpdatedWord { get; set; }
        public string UpdatedMeaning { get; set; }

        public virtual Word Word { get; set; }
    }
}
