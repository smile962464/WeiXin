using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Xml;

using Newtonsoft.Json;

using WeiXin.Models;

namespace avt7.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
		[HttpGet]
        public ActionResult Index()
        {
            return View();
        }

		[HttpPost]
		[ValidateInput(false)]
		public string Index(string inMsg)
		{

			Stream inStream = Request.InputStream;
			XmlDocument doc = new XmlDocument();
			doc.Load(inStream);

			//To = doc.GetElementsByTagName("ToUserName")[0].InnerText;
			//From = doc.GetElementsByTagName("FromUserName")[0].InnerText;
			//Time = Convert.ToInt32(doc.GetElementsByTagName("CreateTime")[0].InnerText);
			//MsgType = doc.GetElementsByTagName("MsgType")[0].InnerText;
			//MsgId = doc.GetElementsByTagName("MsgId")[0].InnerText;

			//TextMessage msg_t = new TextMessage(doc);
			//ImageMessage msg_p = new ImageMessage(doc);
			//LocationMessage msg_lo = new LocationMessage(doc);
			//LinkMessage msg_li = new LinkMessage(doc);
			//EventMessage msg_e = new EventMessage(doc);

			TMessage msg = new TMessage(doc);

			//Message[] msg = new Message[] {msg_t, msg_p, msg_li, msg_lo, msg_e };

			return JsonConvert.SerializeObject(msg.GetMessage());
		}

    }
}
