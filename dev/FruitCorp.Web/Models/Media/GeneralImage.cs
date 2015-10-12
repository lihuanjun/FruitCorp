using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace FruitCorp.Web.Models.Media
{
    [ContentType(DisplayName = "GeneralImage", GUID = "1e1b0e6e-4cb7-4de4-949a-ce8aa106aba8", Description = "")]
    /*[MediaDescriptor(ExtensionString = "pdf,doc,docx")]*/
    public class GeneralImage : ImageData
    {
        public virtual String Description { get; set; }
    }
}