namespace TASKDITASK.ViewModels
{

    public class ProductCreateVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public string? ImagePath { get; set; }
        public int Rating { get; set; }

        public List<int> TagIds { get; set; }
        public IFormFile Image { get; internal set; }
    }
}

