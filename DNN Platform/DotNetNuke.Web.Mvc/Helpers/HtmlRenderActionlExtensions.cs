// Copyright (c) DNN Software. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace DotNetNuke.Web.Mvc.Helpers
{
    /// <summary>
    /// Represents the functionality to render a partial view as an HTML-encoded string.
    /// </summary>
    public static class HtmlRenderActionExtensions
    {
        public static void RenderAction(this DnnHtmlHelper helper, string actionName)
        {
            helper.HtmlHelper.RenderAction(actionName);
        }

        public static void RenderAction(this DnnHtmlHelper helper, string actionName, object routeValues)
        {
            helper.HtmlHelper.RenderAction(actionName, routeValues);
        }

        public static void RenderAction(this DnnHtmlHelper helper, string actionName, RouteValueDictionary routeValues)
        {
            helper.HtmlHelper.RenderAction(actionName, routeValues);
        }

        public static void RenderAction(this DnnHtmlHelper helper, string actionName, string controllerName)
        {
            helper.HtmlHelper.RenderAction(actionName, controllerName);
        }

        public static void RenderAction(this DnnHtmlHelper helper, string actionName, string controllerName, object routeValues)
        {
            helper.HtmlHelper.RenderAction(actionName, controllerName, routeValues);
        }

        public static void RenderAction(this DnnHtmlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            helper.HtmlHelper.RenderAction(actionName, controllerName, routeValues);
        }
    }
}