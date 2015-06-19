using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Infrastructure.Data
{
    /// <summary>
    /// DTO For getting user role information from the metaDepot
    /// </summary>
    public class ApplicationRole
    {
        /// <summary>
        /// Application Id
        /// </summary>
        public int app_id { get; set; }

        /// <summary>
        /// Application Name
        /// </summary>
        public string app_canonical { get; set; }

        /// <summary>
        /// User GUID 
        /// </summary>
        public Guid usr_guid { get; set; }

        /// <summary>
        /// Role ID 
        /// </summary>
        public int ar_num { get; set; }
        
        /// <summary>
        /// Role Name and Value 
        /// </summary>
        public string ar_canonical { get; set; }

 


    }
}
