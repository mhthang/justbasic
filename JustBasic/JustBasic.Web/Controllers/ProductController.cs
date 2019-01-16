using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using JustBasic.Common;
using JustBasic.Model.Models;
using JustBasic.Service;
using JustBasic.Web.Infrastructure.Core;
using JustBasic.Web.Models;

namespace JustBasic.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;
        private IColorService _colorService;
        private ISizeService _sizeService;
        private IPriceService _priceService;

        public ProductController(IProductService productService,
            IProductCategoryService productCategoryService,
            IColorService colorService,
            ISizeService sizeService,
            IPriceService priceService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
            this._colorService = colorService;
            this._sizeService = sizeService;
            this._priceService = priceService;
        }

        // GET: Product
        public ActionResult Detail(int productId)
        {
            var productModel = _productService.GetById(productId);

            var viewModel = Mapper.Map<Product, ProductViewModel>(productModel);

            var relatedProduct = _productService.GetReatedProducts(productId, 6);

            ViewBag.RelatedProducts = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(relatedProduct);

            List<string> listImages = new JavaScriptSerializer().Deserialize<List<string>>(viewModel.MoreImages);
            ViewBag.MoreImages = listImages;

            ViewBag.Tags = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_productService.GetListTagByProductId(productId));

            var ListPriceProduct = _priceService.GetPriceProduct(productId);

            var ListColor = _colorService.GetAllByProduct(productId);

            var ListSize = _sizeService.GetSizeByNotPrice(productId);

            var PriceProductIsPrimary = ListPriceProduct.FirstOrDefault(x => x.IsPrimary);

            //var listSizeID = ListPriceProduct.Where(x => x.ColorID == PriceProductIsPrimary.ColorID).Select(x => x.SizeID).ToList();

            //var SizeByColor = _sizeService.GetAll().Where(x => listSizeID.Any(a => a == x.ID));

            ViewBag.ListPriceProduct = Mapper.Map<IEnumerable<Price>, IEnumerable<PriceViewModel>>(ListPriceProduct);
            ViewBag.ListColor = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(ListColor);
            ViewBag.ListSize = Mapper.Map<IEnumerable<Size>, IEnumerable<SizeViewModel>>(ListSize);

            //ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorService.GetAllByProduct(productId));

            //ViewBag.Sizes = Mapper.Map<IEnumerable<Size>, IEnumerable<SizeViewModel>>(SizeByColor);

            ViewBag.Price = Mapper.Map<Price, PriceViewModel>(PriceProductIsPrimary);

            return View(viewModel);
        }

        public ActionResult All(int page = 1)
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.GetAll();
            totalRow = productModel.Count();
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel.Skip(pageSize * (page - 1)).Take(pageSize),
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public ActionResult Category(int id, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.GetListProductByCategoryIdPaging(id, page, pageSize, sort, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var category = _productCategoryService.GetById(id);
            ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public ActionResult Search(string keyword, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.Search(keyword, page, pageSize, sort, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewBag.Keyword = keyword;
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public ActionResult ListByTag(string tagId, int page = 1)
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.GetListProductByTag(tagId, page, pageSize, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewBag.Tag = Mapper.Map<Tag, TagViewModel>(_productService.GetTag(tagId));
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }

        public JsonResult GetListProductByName(string keyword)
        {
            var model = _productService.GetListProductByName(keyword);
            return Json(new
            {
                data = model
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult t4esst()
        {
            return View();
        }
    }
}