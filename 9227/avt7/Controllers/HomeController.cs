using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avt7.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public void Index()
        {
            //Request.ServerVariables("REMOTE_ADDR");
            string req_ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
            //内网
            if (req_ip.StartsWith("10.") || req_ip.StartsWith("192.") || req_ip.StartsWith("172.") || req_ip.StartsWith("::1"))
            {
                Response.Redirect("http://192.168.203.89:8081/");
            }
            else
            {
                Response.Redirect("http://202.109.244.206:8081/");
                //Response.Redirect("http://www.avt7.com:8081/");
            }
            //return req_ip;
            //return View();
        }

    }
}
