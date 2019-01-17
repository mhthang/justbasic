using AutoMapper;
using JustBasic.Model.Models;
using JustBasic.Service;
using JustBasic.Web.Infrastructure.Core;
using JustBasic.Web.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace JustBasic.Web.Api
{
    [RoutePrefix("api/menu")]
    [Authorize]
    public class menuController : ApiControllerBase
    {
        #region Initialize

        private IMenuService _menuService;

        public menuController(IErrorService errorService, IMenuService menuService)
            : base(errorService)
        {
            this._menuService = menuService;
        }

        #endregion Initialize

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _menuService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(model);

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
                var model = _menuService.GetById(id);

                var responseData = Mapper.Map<Menu, MenuViewModel>(model);

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
                var model = _menuService.GetAll();
                if (!string.IsNullOrEmpty(keyword))
                    model = model.Where(x => x.Name.Contains(keyword));
                totalRow = model.Count();
                var query = model.Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(query.AsEnumerable());

                var paginationSet = new PaginationSet<MenuViewModel>()
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
        public HttpResponseMessage Create(HttpRequestMessage request, MenuViewModel menuCategoryVm)
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
                    var newmenu = new Menu();
                    menuCategoryVm.GroupID = 1;
                    newmenu.UpdateMenu(menuCategoryVm);
                    _menuService.Add(newmenu);
                    _menuService.SaveChanges();

                    var responseData = Mapper.Map<Menu, MenuViewModel>(newmenu);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, MenuViewModel menuVm)
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
                    var dbmenu = _menuService.GetById(menuVm.ID);

                    dbmenu.UpdateMenu(menuVm);
                    _menuService.Update(dbmenu);
                    _menuService.SaveChanges();

                    var responseData = Mapper.Map<Menu, MenuViewModel>(dbmenu);
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
                    var oldmenuCategory = _menuService.Delete(id);
                    _menuService.SaveChanges();

                    var responseData = Mapper.Map<Menu, MenuViewModel>(oldmenuCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedmenus)
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
                    var listmenuCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedmenus);
                    foreach (var item in listmenuCategory)
                    {
                        _menuService.Delete(item);
                    }

                    _menuService.SaveChanges();

                    response = request.CreateResponse(HttpStatusCode.OK, listmenuCategory.Count);
                }

                return response;
            });
        }
    }
}