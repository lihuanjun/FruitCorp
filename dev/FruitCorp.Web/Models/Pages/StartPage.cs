using EPiServer.Core;
using EPiServer.DataAnnotations;
namespace FruitCorp.Web.Models.Pages
{
    [ContentType(DisplayName = "StartPage", GUID = "3d92649b-9806-4c1a-a3cd-1311dcb3fbbd", Description = "")]
    //[AvailablePageTypes(Include = new[] { typeof(StandardPage) })]
    public class StartPage : BasePage
    {
        /// <summary>
        /// 主要内容
        /// </summary>
        public virtual ContentArea Content { get; set; }
    }
}