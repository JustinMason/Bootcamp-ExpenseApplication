using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Bootcamp.Infrastructure.Data
{
    [Table("vwAppUserRole")]
    public class ApplicationUserRole
    {
        [Key]
        public short app_id { get; set; }
        public short usr_id { get; set; }
        public short ar_num { get; set; }
        public string app_description { get; set; }
        public string usr_domainIdentity { get; set; }
        public string ar_description { get; set; }
        public bool enabled { get; set; }
    }
}
