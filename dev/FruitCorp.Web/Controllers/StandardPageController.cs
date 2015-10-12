﻿using System.Web.Mvc;
using EPiServer.Web.Mvc;
using FruitCorp.Web.Models.Pages;

namespace FruitCorp.Web.Controllers
{
    public class StandardPageController : PageController<StandardPage>
    {
        public ActionResult Index(StandardPage currentPage)
        {
            return View(currentPage);
        }
    }
}