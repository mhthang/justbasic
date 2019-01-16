using System.Web.Mvc;

namespace JustBasic.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            //context.MapRoute(
            //    name: "Admin_default",
            //    url: "Admin/{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //      namespaces: new string[] { "JustBasic.Web.Areas.Admin.Controllers" }
            //);
        }
    }
}