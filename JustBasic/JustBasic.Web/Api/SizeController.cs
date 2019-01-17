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
    [RoutePrefix("api/size")]
    [Authorize]
    public class SizeController : ApiControllerBase
    {
        #region Initialize

        private ISizeService _SizeService;

        public SizeController(IErrorService errorService, ISizeService SizeService)
            : base(errorService)
        {
            this._SizeService = SizeService;
        }

        #endregion Initialize

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _SizeService.GetAll();
                //var a = _SizeService.GetSizeByNotPrice(22);
                var responseData = Mapper.Map<IEnumerable<Size>, IEnumerable<SizeViewModel>>(model);

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
                var model = _SizeService.GetById(id);

                var responseData = Mapper.Map<Size, SizeViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getpaging")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _SizeService.GetAll();
                if (!string.IsNullOrEmpty(keyword))
                    model = model.Where(x => x.Name.Contains(keyword));
                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Size>, IEnumerable<SizeViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<SizeViewModel>()
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
        public HttpResponseMessage Create(HttpRequestMessage request, SizeViewModel SizeCategoryVm)
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
                    var newSize = new Size();
                    newSize.UpdateSize(SizeCategoryVm);
                    newSize.CreatedDate = DateTime.Now;
                    newSize.CreatedBy = User.Identity.Name;
                    _SizeService.Add(newSize);
                    _SizeService.SaveChanges();

                    var responseData = Mapper.Map<Size, SizeViewModel>(newSize);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, SizeViewModel SizeVm)
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
                    var dbSize = _SizeService.GetById(SizeVm.ID);

                    dbSize.UpdateSize(SizeVm);
                    dbSize.UpdatedDate = DateTime.Now;
                    dbSize.UpdatedBy = User.Identity.Name;
                    _SizeService.Update(dbSize);
                    _SizeService.SaveChanges();

                    var responseData = Mapper.Map<Size, SizeViewModel>(dbSize);
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
                    var oldSizeCategory = _SizeService.Delete(id);
                    _SizeService.SaveChanges();

                    var responseData = Mapper.Map<Size, SizeViewModel>(oldSizeCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedSizes)
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
                    var listSizeCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedSizes);
                    foreach (var item in listSizeCategory)
                    {
                        _SizeService.Delete(item);
                    }

                    _SizeService.SaveChanges();

                    response = request.CreateResponse(HttpStatusCode.OK, listSizeCategory.Count);
                }

                return response;
            });
        }
    }
}