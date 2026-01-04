using System.ComponentModel.DataAnnotations;
using TASKDITASK.Models.Common;
namespace TASKDITASK.Models;

public class ShippingArea : BaseEntity
{
    
    [MaxLength(15)]
    [MinLength(5)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    [Required]
    public string ImagePath { get; set; } = null!;
}
