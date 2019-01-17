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
    [RoutePrefix("api/color")]
    [Authorize]
    public class colorController : ApiControllerBase
    {
        #region Initialize

        private IColorService _colorService;

        public colorController(IErrorService errorService, IColorService colorService)
            : base(errorService)
        {
            this._colorService = colorService;
        }

        #endregion Initialize

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _colorService.GetAll();
                //var a = _colorService.GetcolorByNotPrice(22);
                var responseData = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(model);

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
                var model = _colorService.GetById(id);

                var responseData = Mapper.Map<Color, ColorViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getpaging")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pagecolor = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _colorService.GetAll();
                if (!string.IsNullOrEmpty(keyword))
                    model = model.Where(x => x.Name.Contains(keyword));

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pagecolor).Take(pagecolor);

                var responseData = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<ColorViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pagecolor)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ColorViewModel colorCategoryVm)
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
                    var newcolor = new Color();
                    newcolor.UpdateColor(colorCategoryVm);
                    newcolor.CreatedDate = DateTime.Now;
                    newcolor.CreatedBy = User.Identity.Name;
                    _colorService.Add(newcolor);
                    _colorService.SaveChanges();

                    var responseData = Mapper.Map<Color, ColorViewModel>(newcolor);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, ColorViewModel colorVm)
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
                    var dbcolor = _colorService.GetById(colorVm.ID);

                    dbcolor.UpdateColor(colorVm);
                    dbcolor.UpdatedDate = DateTime.Now;
                    dbcolor.UpdatedBy = User.Identity.Name;
                    _colorService.Update(dbcolor);
                    _colorService.SaveChanges();

                    var responseData = Mapper.Map<Color, ColorViewModel>(dbcolor);
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
                    var oldcolorCategory = _colorService.Delete(id);
                    _colorService.SaveChanges();

                    var responseData = Mapper.Map<Color, ColorViewModel>(oldcolorCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedcolors)
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
                    var listcolorCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedcolors);
                    foreach (var item in listcolorCategory)
                    {
                        _colorService.Delete(item);
                    }

                    _colorService.SaveChanges();

                    response = request.CreateResponse(HttpStatusCode.OK, listcolorCategory.Count);
                }

                return response;
            });
        }
    }
}