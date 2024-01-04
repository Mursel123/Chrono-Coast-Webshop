using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PXLPRO2023Shoppers29.ViewComponents
{
    public class NavBarComp : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
