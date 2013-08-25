using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using WeiXin;

namespace avt7.Controllers
{
    public class WeiXinController : Controller
    {
        //
        // GET: /WinXin/

        public ActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public ActionResult AccessToken()
		{
			return View();
		}
		[HttpPost]
		public string AccessToken(string appid = "wx8980382cb43a6897", string appsecret = "1137740a67c5152a9cd7af37abb4cd28")
		{
			return Utities.AccessToken(appid, appsecret);
		}

		[HttpGet]
		public ActionResult AddMenu()
		{
			var path = Request.PhysicalApplicationPath + "App_Data\\Menu.js";
			ViewBag.path = path;
			System.IO.FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			StreamReader sr = new StreamReader(fs);
			ViewBag.Menu = sr.ReadToEnd();
			sr.Close();
			fs.Close();
			return View();
		}
		[HttpGet]
		public string _AddMenu(string accessToken)
		{
			var path = Request.PhysicalApplicationPath + "App_Data\\Menu.js";
			ViewBag.path = path;
			return Utities.AddMenuFromFile(accessToken, path);
		}

		[HttpGet]
		public string _GetMenu(string accessToken)
		{
			return Utities.GetMenu(accessToken);
		}

		[HttpGet]
		public string _DeleteMenu(string accessToken)
		{
			return Utities.DeleteMenu(accessToken);
		}

		//TEST PAGE
		[HttpGet]
		public ActionResult Test()
		{
			return View();
		}
    }
}
