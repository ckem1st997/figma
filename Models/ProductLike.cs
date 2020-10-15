using System.ComponentModel.DataAnnotations.Schema;

namespace figma.Models
{
    public class ProductLike
    {
        [Index]
        public int ProductLikeID { get; set; }

        public int ProductID { get; set; }

        public int MemberId { get; set; }

        public virtual Products Products { get; set; }

        public virtual Members Members { get; set; }


    }
}
