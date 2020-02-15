using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace HourglassServer
{
    public class DefaultControllerRoute : Attribute, IRouteTemplateProvider
    {
        public string Template => "[controller]";

        public int? Order { get; set; }

        public string Name { get; set; }
    }
}
