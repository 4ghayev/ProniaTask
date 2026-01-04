using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TASKDITASK.ViewModels
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public int Rating { get; set; }

        public IFormFile? Image { get; set; }  

        public string? ExistingImage { get; set; } 

        public List<int>? TagIds { get; set; }
    }
}
