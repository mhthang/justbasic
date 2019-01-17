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
    [RoutePrefix("api/page")]
    [Authorize]
    public class PageController : ApiControllerBase
    {
        #region Initialize

        private IPageService _PageService;

        public PageController(IErrorService errorService, IPageService PageService)
            : base(errorService)
        {
            this._PageService = PageService;
        }

        #endregion Initialize

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _PageService.GetAll();
                //var a = _PageService.GetPageByNotPrice(22);
                var responseData = Mapper.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(model);

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
                var model = _PageService.GetById(id);

                var responseData = Mapper.Map<Page, PageViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getpaging")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pagePage = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _PageService.GetAll();
                if (!string.IsNullOrEmpty(keyword))
                    model = model.Where(x => x.Name.Contains(keyword));

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pagePage).Take(pagePage);

                var responseData = Mapper.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<PageViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pagePage)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, PageViewModel PageCategoryVm)
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
                    var newPage = new Page();
                    newPage.UpdatePage(PageCategoryVm);
                    newPage.CreatedDate = DateTime.Now;
                    newPage.CreatedBy = User.Identity.Name;
                    _PageService.Add(newPage);
                    _PageService.SaveChanges();

                    var responseData = Mapper.Map<Page, PageViewModel>(newPage);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, PageViewModel PageVm)
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
                    var dbPage = _PageService.GetById(PageVm.ID);

                    dbPage.UpdatePage(PageVm);
                    dbPage.UpdatedDate = DateTime.Now;
                    dbPage.UpdatedBy = User.Identity.Name;
                    _PageService.Update(dbPage);
                    _PageService.SaveChanges();

                    var responseData = Mapper.Map<Page, PageViewModel>(dbPage);
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
                    var oldPageCategory = _PageService.Delete(id);
                    _PageService.SaveChanges();

                    var responseData = Mapper.Map<Page, PageViewModel>(oldPageCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedPages)
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
                    var listPageCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedPages);
                    foreach (var item in listPageCategory)
                    {
                        _PageService.Delete(item);
                    }

                    _PageService.SaveChanges();

                    response = request.CreateResponse(HttpStatusCode.OK, listPageCategory.Count);
                }

                return response;
            });
        }
    }
}