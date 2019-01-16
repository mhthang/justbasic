using JustBasic.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JustBasic.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            //OutputCache.Add(Url, ControllerContext.RouteData, null,
            //                new List<string>() { });

            //var user = User as UserPrincipal;

            DataTable objDataTable = new DataTable();
            string filepathe = "~/Menu/xmlMenuFiles.xml";
            objDataTable = ReadXMLFile(filepathe);

            var _menu = new MenuViewModels();

            foreach (DataRow item in objDataTable.Select("ParentID = 0"))
            {
                MenuItemViewModel menuitem = new MenuItemViewModel()
                {
                    MenuItemName = item["Title"].ToString(),
                    MenuItemPath = item["Action"].ToString(),
                    MenuIcon = item["Icon"].ToString(),
                    MenuController = item["Controller"].ToString(),
                    DisplayOrder = item["DisplayOrder"].ToString(),
                    MenuItemBreadCrumbPath = item["BreadCrumbDefaultLink"].ToString(),
                    BreadCrumbDisplay = item["BreadCrumbDisplay"].ToString().ToLower(),
                    SecondaryTitle = item["SecondaryTitle"].ToString()
                };
                _menu.Items.Add(menuitem);
                getMenuChild(objDataTable, menuitem, item["MenuID"].ToString(), _menu);
            }
            //ViewData["menu"] = _menu;
            return PartialView("_MenuMain", _menu);
        }

        private DataTable ReadXMLFile(string filePath)
        {
            DataSet objds = new DataSet();
            objds.ReadXml(Server.MapPath(filePath));
            return objds.Tables[0];
        }

        private MenuViewModels getMenuChild(DataTable tb, MenuItemViewModel menuitem, string parentID, MenuViewModels menu)
        {
            DataTable tbNews = tb;
            DataRow[] dr = tbNews.Select("ParentID='" + parentID + "'");
            MenuViewModels mn = menu;
            if (dr.Length > 0)
            {
                foreach (DataRow item in dr)
                {
                    MenuItemViewModel itemChild = new MenuItemViewModel()
                    {
                        MenuItemName = item["Title"].ToString(),
                        MenuItemPath = item["Action"].ToString(),
                        MenuIcon = item["Icon"].ToString(),
                        MenuController = item["Controller"].ToString(),
                        DisplayOrder = item["DisplayOrder"].ToString(),
                        MenuItemBreadCrumbPath = item["BreadCrumbDefaultLink"].ToString(),
                        BreadCrumbDisplay = item["BreadCrumbDisplay"].ToString().ToLower(),
                        SecondaryTitle = item["SecondaryTitle"].ToString()
                    };
                    menuitem.ChildMenuItems.Add(itemChild);
                    getMenuChild(tbNews, itemChild, item["MenuID"].ToString(), mn);
                }
            }
            return mn;
        }

        public ActionResult LoadUserLogin(int UserID = 0)
        {
            //OutputCache.Add(Url, ControllerContext.RouteData,
            //                new { UserID = UserID },
            //                new List<string>() { TableKeys.Key_User });

            //UserPrincipal CurrentUser = (UserPrincipal)Session["UserPricinpal"];
            return PartialView();
        }

        public ActionResult _Footer()
        {
            return PartialView("_Footer");
        }
    }
}