using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using JustBasic.Common;
using JustBasic.Model.Models;
using JustBasic.Service;
using JustBasic.Web.App_Start;
using JustBasic.Web.Infrastructure.Extensions;

using JustBasic.Web.Infrastructure.NganLuongAPI;
using JustBasic.Web.Models;

namespace JustBasic.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IProductService _productService;
        private IOrderService _orderService;
        private IPriceService _priceService;
        private ApplicationUserManager _userManager;
        private IColorService _colorService;
        private ISizeService _sizeService;
        private string merchantId = ConfigHelper.GetByKey("MerchantId");
        private string merchantPassword = ConfigHelper.GetByKey("MerchantPassword");
        private string merchantEmail = ConfigHelper.GetByKey("MerchantEmail");

        public ShoppingCartController(IOrderService orderService,
            IProductService productService,
            ApplicationUserManager userManager,
            IColorService colorService,
            ISizeService sizeService,
            IPriceService priceService)
        {
            this._productService = productService;
            this._userManager = userManager;
            this._orderService = orderService;
            this._priceService = priceService;
            this._colorService = colorService;
            this._sizeService = sizeService;
        }

        // GET: ShoppingCart
        //public ActionResult Index()
        //{
        //    Session["NotRegister"] = false;
        //    return View();
        //}

        public ActionResult CheckOut()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                return Redirect("/gio-hang.html");
            }
            return View();
        }

        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        public ActionResult CreateOrder(string orderViewModel)
        {
            Session["NotRegister"] = true;
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);

            var orderNew = new Order();

            orderNew.UpdateOrder(order);

            if (Request.IsAuthenticated)
            {
                orderNew.CustomerId = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            bool isEnough = true;
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.Id;
                detail.Quantity = item.Quantity;
                detail.Price = item.SalePrice - (item.PromotionPrice.HasValue ? item.PromotionPrice.Value : 0);
                detail.Discount = item.PromotionPrice;
                orderDetails.Add(detail);

                //isEnough =_productService.SellProduct(item.ProductId, item.Quantity);
                isEnough = true;
                break;
            }
            if (isEnough)
            {
                orderNew.TotalAmount = orderDetails.Sum(x => x.Price);
                orderNew.TotalDiscount = orderDetails.Sum(x => x.Discount);
                var orderReturn = _orderService.Create(ref orderNew, orderDetails);
                //_productService.Save();

                if (order.PaymentMethod == "CASH" || order.PaymentMethod == "CK")
                {
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    var currentLink = ConfigHelper.GetByKey("CurrentLink");
                    RequestInfo info = new RequestInfo();
                    info.Merchant_id = merchantId;
                    info.Merchant_password = merchantPassword;
                    info.Receiver_email = merchantEmail;

                    info.cur_code = "vnd";
                    info.bank_code = order.BankCode;

                    info.Order_code = orderReturn.ID.ToString();
                    info.Total_amount = orderDetails.Sum(x => x.Quantity * x.Price).ToString();
                    info.fee_shipping = "0";
                    info.Discount_amount = "0";
                    info.order_description = "Thanh toán đơn hàng tại";
                    info.return_url = currentLink + "xac-nhan-don-hang.html";
                    info.cancel_url = currentLink + "huy-don-hang.html";

                    info.Buyer_fullname = order.CustomerName;
                    info.Buyer_email = order.CustomerEmail;
                    info.Buyer_mobile = order.CustomerMobile;

                    APICheckoutV3 objNLChecout = new APICheckoutV3();
                    ResponseInfo result = objNLChecout.GetUrlCheckout(info, order.PaymentMethod);
                    if (result.Error_code == "00")
                    {
                        return Json(new
                        {
                            status = true,
                            urlCheckout = result.Checkout_url,
                            message = result.Description
                        });
                    }
                    else
                        return Json(new
                        {
                            status = false,
                            message = result.Description
                        });
                }
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "Không đủ hàng."
                });
            }
        }

        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new
            {
                data = cart,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(int Id, int Quantity = 1)
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];

            var PriceProduct = _priceService.GetById(Id);

            var PriceProductAll = _priceService.GetAll().ToList();
            //var product = _productService.GetById(PriceProduct.ProductID);
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            //if (product.Quantity == 0)
            //{
            //    return Json(new
            //    {
            //        status = false,
            //        message = "Sản phẩm này hiện đang hết hàng"
            //    });
            //}
            if (cart.Any(x => x.Id == Id))
            {
                foreach (var item in cart)
                {
                    if (item.Id == Id)
                    {
                        item.Quantity += Quantity;
                    }
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();

                Product product = _productService.GetById(PriceProduct.ProductID);
                product.Price = PriceProduct.SalePrice;
                product.PromotionPrice = PriceProduct.PromotionPrice;

                newItem.Product = Mapper.Map<Product, ProductViewModel>(PriceProduct.Product);
                newItem.Id = PriceProduct.ID;
                newItem.ProductId = PriceProduct.Product.ID;
                newItem.Quantity = Quantity;
                newItem.SalePrice = PriceProduct.SalePrice;
                newItem.PromotionPrice = PriceProduct.PromotionPrice;
                newItem.SizeId = PriceProduct.SizeID;
                newItem.ColorId = PriceProduct.ColorID;
                newItem.ColorName = _colorService.GetById(PriceProduct.ColorID).Name;
                newItem.SizeName = _sizeService.GetById(PriceProduct.SizeID).Name;
                cart.Add(newItem);
            }
            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true,
                data = cart
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int Id)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.Id == Id);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true,
                    data = cartSession
                });
            }
            return Json(new
            {
                status = false
            });
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.Id == jitem.Id)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }

            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true,
                data = cartSession
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status = true
            });
        }

        public ActionResult ConfirmOrder()
        {
            string token = Request["token"];
            RequestCheckOrder info = new RequestCheckOrder();
            info.Merchant_id = merchantId;
            info.Merchant_password = merchantPassword;
            info.Token = token;
            APICheckoutV3 objNLChecout = new APICheckoutV3();
            ResponseCheckOrder result = objNLChecout.GetTransactionDetail(info);
            if (result.errorCode == "00")
            {
                //update status order
                _orderService.UpdateStatus(int.Parse(result.order_code));
                _orderService.Save();
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Thanh toán thành công. Chúng tôi sẽ liên hệ lại sớm nhất.";
            }
            else
            {
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Có lỗi xảy ra. Vui lòng liên hệ admin.";
            }
            Session["NotRegister"] = false;
            return View();
        }

        public ActionResult CancelOrder()
        {
            return View();
        }

        public ActionResult Index()
        {
            Session["NotRegister"] = true;
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return View();
        }

        public JsonResult SetNotRegister()
        {
            Session["NotRegister"] = true;
            return Json(new
            {
                status = true
            });
        }
    }
}