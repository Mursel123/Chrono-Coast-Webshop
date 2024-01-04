using Microsoft.Build.Framework;
using System.ComponentModel;

namespace PXLPRO2023Shoppers29.Models.ViewModels
{
    public class BlazorProductViewModel : Product
    {
        [Required]
        [DisplayName("Stock Amount")]
        public int StockAmount { get; set; }


    }
}
