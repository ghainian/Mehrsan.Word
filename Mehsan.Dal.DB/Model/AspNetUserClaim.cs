using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrsan.Dal.DB
{
    [Table("AspNetUserClaims")]
    public class AspNetUserClaim
    {
        public int Id { get; set; }        
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        [ForeignKey("UserId")]
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
