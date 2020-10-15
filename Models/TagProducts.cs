namespace figma.Models
{
    public class TagProducts
    {
        public int TagID { get; set; }
        public virtual Tags Tags { get; set; }
        public int ProductID { get; set; }
        public virtual Products Products { get; set; }

    }
}
