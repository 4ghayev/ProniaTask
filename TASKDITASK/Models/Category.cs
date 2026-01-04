using System.ComponentModel.DataAnnotations;
using TASKDITASK.Models.Common;

namespace TASKDITASK.Models
{
    public class Category:BaseEntity
    {
        [Required]
        [MaxLength]
        public string Name { get; set; }
    }
}
