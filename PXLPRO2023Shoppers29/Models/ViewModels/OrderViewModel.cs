namespace PXLPRO2023Shoppers29.Models.ViewModels
{
    public class OrderViewModel
    {
        public int CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }


        public Payment Payment { get; set; }
        public List<OrderLine> OrderLines { get; set; }

    }
}
