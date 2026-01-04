using System.ComponentModel.DataAnnotations;

namespace TASKDITASK.ViewModels.Tag
{
    public class TagCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
