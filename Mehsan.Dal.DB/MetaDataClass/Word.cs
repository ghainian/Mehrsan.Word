using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrsan.Dal.DB.MetaDataClass
{
    public partial class WordMetadata
    {
        [Required]
        public string TargetWord { get; set; }

        [Required]
        public string Meaning { get; set; }
    }

    [MetadataType(typeof(WordMetadata))]
    public partial class Word
    {
    }
}
