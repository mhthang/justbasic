using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using JustBasic.Common;
using JustBasic.Model.Models;
using JustBasic.Service;
using JustBasic.Web.Infrastructure.Core;
using JustBasic.Web.Models;

namespace JustBasic.Web.Controllers
{
    public class HomeController : Controller
    {
        private IProductCategoryService _productCategoryService;
        private IProductService _productService;
        private ICommonService _commonService;
        private IMenuService _menuService;

        public HomeController(IProductCategoryService productCategoryService,
            IProductService productService,
            IMenuService menuService,
            ICommonService commonService)
        {
            _productCategoryService = productCategoryService;
            _commonService = commonService;
            _productService = productService;
            _menuService = menuService;
        }

        [OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var slideModel = _commonService.GetSlides();
            var homeViewModel = new HomeViewModel();
            var slideView = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(slideModel);
            homeViewModel.Slides = slideView;

            var model = _productCategoryService.GetAll();
            var listProductCategoryViewModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            homeViewModel.listProductCategory = listProductCategoryViewModel;
            try
            {
                homeViewModel.Title = _commonService.GetSystemConfig(CommonConstants.HomeTitle).ValueString;
                homeViewModel.MetaKeyword = _commonService.GetSystemConfig(CommonConstants.HomeMetaKeyword).ValueString;
                homeViewModel.MetaDescription = _commonService.GetSystemConfig(CommonConstants.HomeMetaDescription).ValueString;
            }
            catch
            {
                homeViewModel.Title = "Just Basic";
                homeViewModel.MetaKeyword = "VNXK";
                homeViewModel.MetaDescription = "Just Basic - hàng vn xuất khẩu";
            }

            return View(homeViewModel);
        }

        //[OutputCache(Duration = 3600)]
        //public ActionResult Design()
        //{
        //    var category = _productCategoryService.GetAll().OrderByDescending(x => x.UpdatedDate).Where(x => x.TypeID == 2 && x.HomeFlag == true).FirstOrDefault();
        //    if (category == null)
        //        category = _productCategoryService.GetAll().OrderByDescending(x => x.UpdatedDate).Where(x => x.TypeID == 1).FirstOrDefault();
        //    int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
        //    int totalRow = 0;
        //    var productModel = _productService.GetListProductByCategoryIdPaging(category.ID, 0, pageSize, "", out totalRow);
        //    var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
        //    int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

        //    ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
        //    var paginationSet = new PaginationSet<ProductViewModel>()
        //    {
        //        Items = productViewModel,
        //        MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
        //        Page = 0,
        //        TotalCount = totalRow,
        //        TotalPages = totalPage
        //    };

        //    return View(paginationSet);
        //}

        public ActionResult Product()
        {
            var category = _productCategoryService.GetAll().OrderByDescending(x => x.UpdatedDate).Where(x => x.HomeFlag == true).FirstOrDefault();
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.GetListProductByCategoryIdPaging(category.ID, 0, pageSize, "", out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = 0,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        //[OutputCache(Duration = 3600)]
        public ActionResult Service()
        {
            return View();
        }

        //[OutputCache(Duration = 3600)]
        public ActionResult Order()
        {
            return View();
        }

        [ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public ActionResult Footer()
        {
            //var footerModel = _commonService.GetFooter();
            //var footerViewModel = Mapper.Map<Footer, FooterViewModel>(footerModel);
            return PartialView();
        }


        public ActionResult SideBar()
        {
            return PartialView();
        }


        public ActionResult BestSeller()
        {
            var hotProduct = _productService.GetHotProduct(100);
            var hotProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(hotProduct);
            return PartialView("SlideProduct", hotProductViewModel);
        }

        public ActionResult Related(int ProductID)
        {
            var hotProduct = _productService.GetReatedProducts(ProductID, 10);
            var hotProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(hotProduct);
            return PartialView("SlideProduct", hotProductViewModel);
        }

        public ActionResult GetSystemConfig(string Name)
        {
            var footerModel = _commonService.GetConfig(Name);
            if (footerModel == null)
                footerModel = new Footer();
            var footerViewModel = Mapper.Map<Footer, FooterViewModel>(footerModel);
            return PartialView("_SystemConfig", footerViewModel);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            //var slideModel = _commonService.GetSlides();
            //var slideView = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(slideModel);
            return PartialView();
        }

        [ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public ActionResult Menu()
        {
            var MenuModel = _menuService.GetAll().Where(x => x.Status).OrderBy(x => x.DisplayOrder);
            var menuView = Mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(MenuModel);
            return PartialView(menuView);
        }

        [ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public ActionResult Slide()
        {
            var slideModel = _commonService.GetSlides();
            var slideView = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(slideModel);
            return PartialView(slideView);
        }

        [ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public ActionResult Category()
        {
            var model = _productCategoryService.GetAll();
            var listProductCategoryViewModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryViewModel);
        }

        [ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public ActionResult Category_Design()
        {
            var model = _productCategoryService.GetAll();
            var listProductCategoryViewModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView("Category", listProductCategoryViewModel);
        }

        [HttpPost]
        public JsonResult Upload()
        {
            string fileName = Request.Headers["X-File-Name"];
            string fileType = Request.Headers["X-File-Type"];
            int fileSize = Convert.ToInt32(Request.Headers["X-File-Size"]);
            //File's content is available in Request.InputStream property
            System.IO.Stream fileContent = Request.InputStream;
            //Creating a FileStream to save file's content
            System.IO.FileStream fileStream = System.IO.File.Create(Server.MapPath("~/UploadedFiles/images/") + fileName);
            fileContent.Seek(0, System.IO.SeekOrigin.Begin);
            //Copying file's content to FileStream
            fileContent.CopyTo(fileStream);
            fileStream.Dispose();
            return Json("File uploaded");
        }

        [HttpPost]
        public JsonResult SendMail()
        {
            string fileName = Request.Headers["X-File-Name"];
            string fileType = Request.Headers["X-File-Type"];
            int fileSize = Convert.ToInt32(Request.Headers["X-File-Size"]);
            //File's content is available in Request.InputStream property
            System.IO.Stream fileContent = Request.InputStream;
            ////Creating a FileStream to save file's content
            //System.IO.FileStream fileStream = System.IO.File.Create(Server.MapPath("~/UploadedFiles/images/") + fileName);
            //fileContent.Seek(0, System.IO.SeekOrigin.Begin);
            ////Copying file's content to FileStream
            //fileContent.CopyTo(fileStream);
            //fileStream.Dispose();
            Attachment attachment = new Attachment(fileContent, fileName);
            MailHelper.SendMail("JustBasic.info@gmail.com", "Đăng ký thành công", "Upload file", attachment);
            return Json(new
            {
                statusCode = 200,
                status = "Gửi thành công."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegisterReceivePromotion(string txtEmail)
        {
            try
            {
                var adminEmail = ConfigHelper.GetByKey("AdminEmail");
                var addr = new System.Net.Mail.MailAddress(txtEmail);
                MailHelper.SendMail(adminEmail, "Đăng ký thành công", "Đăng ký nhận email khuyến mãi: <b>" + txtEmail + "</b>");
                return Json(new
                {
                    statusCode = 200,
                    status = "Gửi thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    statusCode = 406,
                    status = "Email không đúng, vui lòng kiểm tra lại!."
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}