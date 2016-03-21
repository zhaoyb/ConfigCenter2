using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConfigCenter.Test.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public string Index()
        {
            string result;
            if (ConfigurationManager.AppSettings["a"] != null)
            {
                result = ConfigurationManager.AppSettings["a"];
            }
            else
            {
                result = "未检测到a节点";
            }

            return result;
        }
    }
}