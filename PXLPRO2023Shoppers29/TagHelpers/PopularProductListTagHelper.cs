using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PXLPRO2023Shoppers29.Blazor;
using PXLPRO2023Shoppers29.Models;

namespace PXLPRO2023Shoppers29.TagHelpers
{
    
    [HtmlTargetElement("PopularProductList")]
    public class PopularProductListTagHelper : TagHelper
    {
        public IEnumerable<Models.Product> Producten { get; set; }

        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            var divElement = new TagBuilder("div");
            divElement.AddCssClass("row"); // Add a container div with class "row" to hold the generated products

            foreach (var item in Producten)
            {
                var productElement = new TagBuilder("div");
                productElement.AddCssClass("col-xl-3 col-lg-4 col-md-6 col-12");

                var innerHtml = $@"
                        <div class=""mirora-product mb-md--10"">
                            <div class=""product-img d-flex justify-content-center align-items-center"">
                                <img src=""{item.ImageUrl}"" alt=""{item.Name}"" class=""primary-image m-0"" height=""250"" />
                                <img src=""{item.ImageUrl}"" alt=""{item.Name}"" class=""secondary-image m-0"" height=""200"" />


                            </div>
                            <div class=""product-content text-center"">
                                <h4>{item.Name}</h4>
                                <div class=""product-price-wrapper"">
                                    <span class=""money"">${item.Price}</span>
                                </div>
                            </div>
                        </div>";

                productElement.InnerHtml.AppendHtml(innerHtml);
                divElement.InnerHtml.AppendHtml(productElement);
            }

            output.TagName = "div";
            output.Content.AppendHtml(divElement);
        }

    }
    }
