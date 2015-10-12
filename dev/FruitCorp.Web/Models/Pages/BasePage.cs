using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace FruitCorp.Web.Models.Pages
{
    [ContentType(DisplayName = "BasePage", 
        GUID = "86d141f1-6341-4c08-bbe8-0d5136c3bcc5", 
        Description = "")]
    public abstract class BasePage : PageData
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }
        
        /// <summary>
        /// meta descript for seo
        /// </summary>
        [UIHint(UIHint.Textarea)]
        public virtual string MetaDescription { get; set; }
    }
}