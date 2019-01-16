using System.Collections.Generic;

namespace JustBasic.Web.Areas.Admin.Models
{
    public class HelpTreeViewModels
    {
        public HelpTreeViewModels()
        {
            TreeItemViewModels = new List<TreeItemViewModels>();
        }

        public List<TreeItemViewModels> TreeItemViewModels;
    }

    public class TreeItemViewModels
    {
        public TreeItemViewModels()
        {
            //this.ChildItems = new List<TreeItemViewModels>();
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        //public virtual List<TreeItemViewModels> ChildItems { get; set; }
    }
}