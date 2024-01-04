namespace PXLPRO2023Shoppers29.Models.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Material { get; set; }
        public string Bezel { get; set; }
        public string SerialNumber { get; set; }
        public string MadeDate { get; set; }
        public string Dial { get; set; }
        public string Bracelet { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public Task<List<Product>> Products { get; set; }
    }

}
