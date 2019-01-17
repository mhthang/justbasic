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
    [RoutePrefix("api/footer")]
    [Authorize]
    public class FooterController : ApiControllerBase
    {
        #region Initialize

        private IFooterService _FooterService;

        public FooterController(IErrorService errorService, IFooterService FooterService)
            : base(errorService)
        {
            this._FooterService = FooterService;
        }

        #endregion Initialize

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _FooterService.GetAll();
                //var a = _FooterService.GetFooterByNotPrice(22);
                var responseData = Mapper.Map<IEnumerable<Footer>, IEnumerable<FooterViewModel>>(model);

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
                var model = _FooterService.GetById(id);

                var responseData = Mapper.Map<Footer, FooterViewModel>(model);

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
                var model = _FooterService.GetAll();
                if (!string.IsNullOrEmpty(keyword))
                    model = model.Where(x => x.Name.Contains(keyword));
                totalRow = model.Count();
                var query = model.Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Footer>, IEnumerable<FooterViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<FooterViewModel>()
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
        public HttpResponseMessage Create(HttpRequestMessage request, FooterViewModel FooterCategoryVm)
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
                    var newFooter = new Footer();
                    newFooter.UpdateFooter(FooterCategoryVm);
                    _FooterService.Add(newFooter);
                    _FooterService.SaveChanges();

                    var responseData = Mapper.Map<Footer, FooterViewModel>(newFooter);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, FooterViewModel FooterVm)
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
                    var dbFooter = _FooterService.GetById(FooterVm.ID);

                    dbFooter.UpdateFooter(FooterVm);
                    _FooterService.Update(dbFooter);
                    _FooterService.SaveChanges();

                    var responseData = Mapper.Map<Footer, FooterViewModel>(dbFooter);
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
                    var oldFooterCategory = _FooterService.Delete(id);
                    _FooterService.SaveChanges();

                    var responseData = Mapper.Map<Footer, FooterViewModel>(oldFooterCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedFooters)
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
                    var listFooterCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedFooters);
                    foreach (var item in listFooterCategory)
                    {
                        _FooterService.Delete(item);
                    }

                    _FooterService.SaveChanges();

                    response = request.CreateResponse(HttpStatusCode.OK, listFooterCategory.Count);
                }

                return response;
            });
        }
    }
}