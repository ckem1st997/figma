using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class OrderInfo
    {


        [Key]
        public decimal OrderId { get; set; }

        [Required(ErrorMessage = "Xin bạn vui lòng nhập số tiền thanh toán")]

        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Xin bạn vui lòng nhập nội dung thanh toán")]

        public string OrderDescription { get; set; }
        public string OrderCatory { get; set; }
        public string bank { get; set; }
        public string language { get; set; }

        public string BankCode { get; set; }


        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal vnp_TransactionNo { get; set; }
        public string vpn_Message { get; set; }
        public string vpn_TxnResponseCode { get; set; }
    }
}
