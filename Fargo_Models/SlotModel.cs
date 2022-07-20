using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class SlotModel
    {
        public long SLOT_ID { get; set; }
        public string SLOT_NAME { get; set; }
        public string STORE_NAME { get; set; }
        public string SLOT_CODE { get; set; }
        public long STORE_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public string FROM_TRACKING_NO { get; set; }
        public string TO_TRACKING_NO { get; set; }
        public string CURRENT_TRACKING_NO { get; set; }
        public long USER_ID { get; set; }
    }
}
