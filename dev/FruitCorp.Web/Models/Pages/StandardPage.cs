using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace FruitCorp.Web.Models.Pages
{
    [ContentType(DisplayName = "StandardPage", GUID = "9238d9f0-71de-4860-ab2c-7b3fb5658219", Description = "")]
    public class StandardPage : PageData
    {
        /// <summary>
        /// 主体介绍
        /// </summary>
        public virtual string MainIntro { get; set; }

        /// <summary>
        /// 主要内容
        /// </summary>
        public virtual XhtmlString MainBody { get; set; }
    }
}