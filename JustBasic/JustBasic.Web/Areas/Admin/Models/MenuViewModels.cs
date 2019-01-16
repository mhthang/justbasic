using System.Collections.Generic;

namespace JustBasic.Web.Areas.Admin.Models
{
    public class MenuViewModels
    {
        public MenuViewModels()
        {
            Items = new List<MenuItemViewModel>();
        }

        public List<MenuItemViewModel> Items;
    }

    public class MenuItemViewModel
    {
        public MenuItemViewModel()
        {
            this.ChildMenuItems = new List<MenuItemViewModel>();
        }

        public string DisplayOrder { get; set; }
        public string MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public string MenuItemPath { get; set; }
        public string MenuItemBreadCrumbPath { get; set; }
        public string BreadCrumbDisplay { get; set; }
        public string MenuIcon { get; set; }
        public string MenuController { get; set; }
        public string ParentItemId { get; set; }
        public string SecondaryTitle { get; set; }
        public virtual List<MenuItemViewModel> ChildMenuItems { get; set; }
    }
}