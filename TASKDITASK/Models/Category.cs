using System.ComponentModel.DataAnnotations;
using TASKDITASK.Models.Common;

namespace TASKDITASK.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
