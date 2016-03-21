using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ConfigCenter.Business;
using ConfigCenter.Dto;
using Webdiyer.WebControls.Mvc;

namespace ConfigCenter.Admin.Controllers
{
    public class AppController : Controller
    {
        // GET: App
        public ActionResult Index(int pageindex = 1, string kword = "")
        {
            long totalItem;
            var dto = AppBusiness.GetApps(pageindex, 2, out totalItem);
            return View(new PagedList<AppDto>(dto, pageindex, 2, (int)totalItem));
        }

        public JsonResult GetAppById(int id)
        {
            return Json(AppBusiness.GetAppById(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAppById(int id)
        {
            ResponseResult responseResult;
            try
            {
                var result = AppBusiness.DeleteAppById(id);
                responseResult = new ResponseResult(result, "");
            }
            catch (Exception)
            {
                responseResult = new ResponseResult(false, "");
            }
            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveApp(AppDto appDto)
        {
            ResponseResult responseResult;
            try
            {
                AppBusiness.SaveApp(appDto);
                responseResult = new ResponseResult(true, "");
            }
            catch (Exception)
            {
                responseResult = new ResponseResult(false, "");
            }
            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }
    }
}
