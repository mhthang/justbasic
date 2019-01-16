using System.Web.Mvc;

namespace JustBasic.Web.Areas.Admin.Controllers
{
    public class ProductCategoriesController : Controller
    {
        //private readonly IProductCategoryService _IProductCategoryService;

        //private CustomPrincipal _UserPrincipal
        //{
        //    get { return User as CustomPrincipal; }
        //}

        //public ProductCategoriesController(IProductCategoryService IProductCategoryService)
        //{
        //    _IProductCategoryService = IProductCategoryService;
        //}

        public ActionResult Index()
        {
            //_IProductCategoryService
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}