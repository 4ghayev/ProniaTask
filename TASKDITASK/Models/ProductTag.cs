using TASKDITASK.Models.Common;

namespace TASKDITASK.Models
{
    public class ProductTag : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
