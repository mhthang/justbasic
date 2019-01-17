using AutoMapper;
using JustBasic.Model.Models;
using JustBasic.Service;
using JustBasic.Web.Infrastructure.Core;
using JustBasic.Web.Infrastructure.Extensions;
using JustBasic.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace JustBasic.Web.Api
{
    [RoutePrefix("api/price")]
    [Authorize]
    public class priceController : ApiControllerBase
    {
        #region Initialize

        private IPriceService _priceService;

        public priceController(IErrorService errorService, IPriceService priceService)
            : base(errorService)
        {
            this._priceService = priceService;
        }

        #endregion Initialize

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _priceService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Price>, IEnumerable<PriceViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            };
            return CreateHttpResponse(request, func);
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _priceService.GetById(id);

                var responseData = Mapper.Map<Price, PriceViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _priceService.GetAll();
                if (!string.IsNullOrEmpty(keyword))
                    model = model.Where(x => x.Product.Name.Contains(keyword)
                    || x.Size.Name.Contains(keyword)
                    || x.Color.Name.Contains(keyword));

                totalRow = model.Count();
                var query = model.Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Price>, IEnumerable<PriceViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<PriceViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, PriceViewModel priceCategoryVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newprice = new Price();
                    newprice.UpdatePrice(priceCategoryVm);

                    var PriceExists = _priceService.GetPrice(newprice.ProductID, newprice.SizeID, newprice.ColorID);
                    if (PriceExists != null && PriceExists.ID > 0)
                    {
                        response = request.CreateResponse(HttpStatusCode.BadGateway, new { ErrorMesage = "Sản phẩm đã được làm giá." });
                    }
                    else
                    {
                        _priceService.Add(newprice);
                        if (newprice.IsPrimary)
                        {
                            var pricePrimary = new Price();
                            pricePrimary = _priceService.GetByPrimary(newprice.ProductID);
                            if (pricePrimary != null && pricePrimary.ProductID > 0)
                            {
                                pricePrimary.IsPrimary = false;
                                _priceService.Update(pricePrimary);
                            }
                        }
                        else
                        {
                            var priceProduct = _priceService.GetPriceProduct(newprice.ProductID);
                            if (priceProduct == null || priceProduct.Count() == 0)
                            {
                                newprice.IsPrimary = true;
                            }
                        }
                        _priceService.SaveChanges();

                        var responseData = Mapper.Map<Price, PriceViewModel>(newprice);
                        response = request.CreateResponse(HttpStatusCode.Created, responseData);
                    }
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, PriceViewModel priceVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbprice = _priceService.GetById(priceVm.ID);
                    if (priceVm.IsPrimary)
                    {
                        var pricePrimary = new Price();
                        pricePrimary = _priceService.GetByPrimary(priceVm.ProductID);
                        if (pricePrimary != null && pricePrimary.ProductID > 0)
                        {
                            pricePrimary.IsPrimary = false;
                            _priceService.Update(pricePrimary);
                        }
                    }
                    dbprice.UpdatePrice(priceVm);
                    _priceService.Update(dbprice);
                    _priceService.SaveChanges();

                    var responseData = Mapper.Map<Price, PriceViewModel>(dbprice);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var oldpriceid = _priceService.Delete(id);
                    _priceService.SaveChanges();

                    var responseData = Mapper.Map<Price, PriceViewModel>(oldpriceid);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedprices)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listpriceCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedprices);
                    foreach (var item in listpriceCategory)
                    {
                        _priceService.Delete(item);
                    }

                    _priceService.SaveChanges();

                    response = request.CreateResponse(HttpStatusCode.OK, listpriceCategory.Count);
                }

                return response;
            });
        }
    }
}