using System.Web.Mvc;
using System.Linq;
using System.Collections;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using EPiServer.Filters;

namespace FruitCorp.Web.Helpers
{
    public static class NavigationHelpers
    {
        /// <summary>
        /// 输出上方一级主导航
        /// </summary>
        /// <param name="html"></param>
        /// <param name="includeRoot">是否包含根节点</param>
        /// <param name="rootLink">根节点对象</param>
        /// <param name="contentLink">当前节点对象</param>
        /// <param name="contentLoader">节点加载器</param>
        public static void RenderMainNavigation(
            this HtmlHelper html,
            bool includeRoot = true,
            PageReference rootLink = null,
            ContentReference contentLink = null,            
            IContentLoader contentLoader = null) 
        {
            rootLink = rootLink ?? ContentReference.StartPage;
            contentLink = contentLink ?? html.ViewContext.RequestContext.GetContentLink(); //GetContentLink()方法必须引用EPiServer.Web.Routing
            contentLoader = contentLoader ?? ServiceLocator.Current.GetInstance<IContentLoader>();
            

            var writer = html.ViewContext.Writer;
            writer.WriteLine("<nav class=\"navbar navbar-inverse\">");
            writer.WriteLine("<ul class=\"nav navbar-nav\">");

            if (includeRoot) 
            {
                //Link to the start page
                writer.WriteLine(rootLink.CompareToIgnoreWorkID(contentLink) ? "<li class=\"active\">" : "<li>");
                writer.WriteLine(html.PageLink(rootLink).ToHtmlString());
                writer.WriteLine("</li>");
            }

            //Link to start pages children            
            var topLevelPages = contentLoader.GetChildren<PageData>(rootLink);

            var currentBranch = contentLoader.GetAncestors(contentLink).Select(x => x.ContentLink).ToList();
            currentBranch.Add(contentLink);
            foreach (var topLevelPage in topLevelPages)
            {
                writer.WriteLine(currentBranch.Any(x => x.CompareToIgnoreWorkID(topLevelPage.ContentLink)) ? "<li class=\"active\">" : "<li>");

                writer.WriteLine(html.PageLink(topLevelPage).ToHtmlString());
                writer.WriteLine("</li>");
            }

            writer.WriteLine("</ul>");
            writer.WriteLine("</nav>");
        }

        /// <summary>
        /// 输出左侧栏目二级导航，可递归
        /// </summary>
        /// <param name="html"></param>
        /// <param name="contentLink"></param>
        /// <param name="contentLoader"></param>
        public static void RenderSubNavigation(
            this HtmlHelper html,
            ContentReference contentLink = null,
            IContentLoader contentLoader = null
            )
        {
            var rootLink = ContentReference.StartPage;
            contentLink = contentLink ?? html.ViewContext.RequestContext.GetContentLink();
            contentLoader = contentLoader ?? ServiceLocator.Current.GetInstance<IContentLoader>();

            var path = contentLoader.GetAncestors(contentLink).Reverse().SkipWhile(x => ContentReference.IsNullOrEmpty(x.ParentLink) || !x.ParentLink.CompareToIgnoreWorkID(rootLink))
                .OfType<PageData>()
                .Select(x => x.PageLink)
                .ToList();

            //var path = NavigationPath(contentLink, contentLoader, rootLink).Select(x => x.PageLink);

            var currentPage = contentLoader.Get<IContent>(contentLink) as PageData;
            if (currentPage != null)
            {
                path.Add(currentPage.PageLink);
            }

            var root = path.FirstOrDefault();
            if (root == null)
            {
                return;
            }
            RenderSubNavigationLevel(html, root, path, contentLoader);
        }
        /// <summary>
        /// 递归二级子导航
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="levelRootLink"></param>
        /// <param name="path"></param>
        /// <param name="contentLoader"></param>
        private static void RenderSubNavigationLevel(
            HtmlHelper helper,
            ContentReference levelRootLink,
            IEnumerable<ContentReference> path,
            IContentLoader contentLoader
            )
        {
            var children = contentLoader.GetChildren<PageData>(levelRootLink);
            children = FilterForVisitor.Filter(children).OfType<PageData>().Where(x => x.VisibleInMenu);
            if (!children.Any())
            {
                return;
            }
            var writer = helper.ViewContext.Writer;
            writer.WriteLine("<ul class=\"nav\">");

            var indexedChildren = children.Select((page, index) => new { index, page }).ToList();

            foreach (var levelItem in indexedChildren)
            {
                var page = levelItem.page;
                var partOfCurrentBranch = path.Any(x => x.CompareToIgnoreWorkID(levelItem.page.ContentLink));
                writer.WriteLine(partOfCurrentBranch ? "<li class=\"active\">" : "<li>");
                writer.WriteLine(helper.PageLink(page).ToHtmlString());
                if (partOfCurrentBranch)
                {
                    RenderSubNavigationLevel(helper, page.ContentLink, path, contentLoader);
                }
                writer.WriteLine("</li>");
            }

            writer.WriteLine("</ul>");
        }

        /// <summary>
        /// 输出面包屑路径
        /// </summary>
        /// <param name="html"></param>
        /// <param name="contentLink"></param>
        /// <param name="contentLoader"></param>
        public static void RenderBreadcrumb(
            this HtmlHelper html,
            ContentReference contentLink = null,
            IContentLoader contentLoader = null
            )
        {
            contentLink = contentLink ?? html.ViewContext.RequestContext.GetContentLink();
            contentLoader = contentLoader ?? ServiceLocator.Current.GetInstance<IContentLoader>();

            var pagePath = NavigationPath(contentLink, contentLoader);
            var path = FilterForVisitor.Filter(pagePath)
                .OfType<PageData>()
                .Select(x => x.PageLink);
            if (!path.Any())
            {
                return;
            }

            var writer = html.ViewContext.Writer;
            writer.WriteLine("<ol class=\"breadcrumb\">");

            foreach (var part in path)
            {
                if (part.CompareToIgnoreWorkID(contentLink))
                {
                    writer.WriteLine("<li class=\"active\">");
                    var currentPage = contentLoader.Get<PageData>(contentLink);
                    writer.WriteLine(html.Encode(currentPage.PageName));
                }
                else
                {
                    writer.WriteLine("<li>");
                    writer.WriteLine(html.PageLink(part));
                }

                writer.WriteLine("</li>");
            }

            writer.WriteLine("</ol>");
        }
       
        /// <summary>
        /// 面包屑路径递归
        /// </summary>
        /// <param name="contentLink"></param>
        /// <param name="contentLoader"></param>
        /// <param name="fromLink"></param>
        /// <returns></returns>
        private static IEnumerable<PageData> NavigationPath(
            ContentReference contentLink,
            IContentLoader contentLoader,
            ContentReference fromLink = null) 
        {
            fromLink = fromLink ?? ContentReference.RootPage;

            var path = contentLoader.GetAncestors(contentLink)
                .Reverse()
                .SkipWhile(x =>
                ContentReference.IsNullOrEmpty(x.ParentLink)
                || !x.ParentLink.CompareToIgnoreWorkID(fromLink))
                .OfType<PageData>()
                .ToList();

            var currentPage = contentLoader.Get<IContent>(contentLink) as PageData;
            if (currentPage != null) 
            {
                path.Add(currentPage);
            }
            return path;
        }

        
    }
}