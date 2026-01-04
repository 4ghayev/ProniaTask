using System.ComponentModel.DataAnnotations;

public class CategoryCreateVM
{
    [Required]
    public string Name { get; set; }
}