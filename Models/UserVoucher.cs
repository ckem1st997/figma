using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class UserVoucher
    {
        [Index]
        public int id { get; set; }
        [StringLength(50)]
        public string MaDonHang { get; set; }

        public decimal SumHD { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập thông tin"), MaxLength(6, ErrorMessage = "Độ dài Voucher phải là 6 nha !")]
        [Display(Name = "Mã giảm giá")]
        [DataType(DataType.Text)]
        public string Code { get; set; }

    }
}
