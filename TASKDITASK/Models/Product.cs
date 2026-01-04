using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TASKDITASK.Models.Common;

namespace TASKDITASK.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name  { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Precision(10,2)]
       [Range(0,double.MaxValue)]
        public decimal Price { get; set; }

        public string ImagePath { get; set; }
        public Category? Category { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public int Rating { get; set; }
    }
}
