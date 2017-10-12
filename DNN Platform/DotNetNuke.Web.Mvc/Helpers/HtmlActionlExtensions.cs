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
    public static class HtmlActionExtensions
    {
        public static MvcHtmlString Action(this DnnHtmlHelper helper, string actionName)
        {
            return helper.HtmlHelper.Action(actionName);
        }

        public static MvcHtmlString Action(this DnnHtmlHelper helper, string actionName, object routeValues)
        {
            return helper.HtmlHelper.Action(actionName, routeValues);
        }

        public static MvcHtmlString Action(this DnnHtmlHelper helper, string actionName, RouteValueDictionary routeValues)
        {
            return helper.HtmlHelper.Action(actionName, routeValues);
        }

        public static MvcHtmlString Action(this DnnHtmlHelper helper, string actionName, string controllerName)
        {
            return helper.HtmlHelper.Action(actionName, controllerName);
        }

        public static MvcHtmlString Action(this DnnHtmlHelper helper, string actionName, string controllerName, object routeValues)
        {
            return helper.HtmlHelper.Action(actionName, controllerName, routeValues);
        }

        public static MvcHtmlString Action(this DnnHtmlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            return helper.HtmlHelper.Action(actionName, controllerName, routeValues);
        }
    }
}