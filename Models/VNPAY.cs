using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class VNPAY
    {
        public string vnp_Url { get; set; }
        public string vnp_Returnurl { get; set; }
        public string vnpay_api_url { get; set; }
        public string vnp_TmnCode { get; set; }
        public string vnp_HashSecret { get; set; }
    }
}
