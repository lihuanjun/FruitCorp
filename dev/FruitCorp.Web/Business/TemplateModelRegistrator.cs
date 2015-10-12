using System;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using FruitCorp.Web.Models.Pages;
using EPiServer.DataAbstraction;

namespace FruitCorp.Web.Business
{
    [ServiceConfiguration(typeof(IViewTemplateModelRegistrator))]
    public class TemplateModelRegistrator : IViewTemplateModelRegistrator
    {
        public void Register(TemplateModelCollection viewTemplateModelRegistrator) 
        {
            var compactStandardPagePartial = new TemplateModel 
            {
                Name = "StandardPageCompact",
                Path = "~/Views/Shared/PagePartials/StandardPageCompact.cshtml",
                Tags = new[] { "StartPage" }
            };

            viewTemplateModelRegistrator.Add(typeof(StandardPage), compactStandardPagePartial);
        }
    }
}