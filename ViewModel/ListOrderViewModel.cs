using figma.Models;
using figma.OutFile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace figma.ViewModel
{
    public class OrderViewModel
    {
        public Order Order { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderDetailProduct
    {
        public Products Products { get; set; }
        public int Quantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? Price { get; set; }
    }
    public class ListOrderViewModel
    {
        public PaginatedList<Order> Orders { get; set; }

        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string MaDonhang { get; set; }
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string CustomerName { get; set; }
        [EmailAddress(ErrorMessage = "Email không hợp lệ"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string CustomerEmail { get; set; }
        [StringLength(20, ErrorMessage = "Tối đa 20 ký tự")]
        public string CustomerMobile { get; set; }
        public int Status { get; set; }
        public int Payment { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        [Required]
        public int PageSize { get; set; }
    }
}
