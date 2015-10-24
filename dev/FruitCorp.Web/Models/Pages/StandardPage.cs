using EPiServer.Core;
using System.ComponentModel.DataAnnotations;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using EPiServer.Framework.DataAnnotations;
using EPiServer;

namespace FruitCorp.Web.Models.Pages
{
    [ContentType(DisplayName = "StandardPage", GUID = "9238d9f0-71de-4860-ab2c-7b3fb5658219", Description = "")]
    [ImageUrl("~/Static/Icon/xx.png")]
    public class StandardPage : BasePage
    {
        /// <summary>
        /// 主体介绍
        /// </summary>
        [UIHint("TextareaLineBreak")]
        //[UIHint(UIHint.Textarea)]
        [UIHint(UIHint.Textarea, PresentationLayer.Edit)]
        public virtual string MainIntro { get; set; }

        [UIHint(UIHint.Image)]
        public virtual Url Image { get; set; }

        /// <summary>
        /// 主要内容
        /// </summary>
        public virtual XhtmlString MainBody { get; set; }

        public virtual ContentArea RightColumnContent { get; set; }
    }
}