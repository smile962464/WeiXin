using avt7.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;

using WeiXin;
using WeiXin.Models;

using XiamenAir.DBTools;
using RegExp = System.Text.RegularExpressions;

namespace avt7.Controllers
{
	public class avt7Controller : Controller
	{
		//Get : avt7
		[HttpGet]
		public string Index(string signature, string timestamp, string nonce, string echostr)
		{
			//Utities.Log(appPath + "weixinLog/sig_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".txt", echostr);
			return echostr;
		}

		//Post : avt7
		[HttpPost]
		[ValidateInput(false)]
		public string Index()
		{
			var appPath = Request.PhysicalApplicationPath;
			try
			{
				Stream input = Request.InputStream;

				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(Request.InputStream);
				Utities.Log(appPath + "weixinLog/msg_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".txt", xmlDoc.InnerXml);

				TMessage MSG = new TMessage(xmlDoc);

				MessageHandlers handlers = new MessageHandlers(this.Response.OutputStream);
				handlers.AddMessageHandler("text", new TextMsgHandler());
				handlers.AddMessageHandler("image", new ImageMsgHandler());
				handlers.AddMessageHandler("location", new LocationMsgHandler());
				handlers.AddMessageHandler("event", new EventMsgHandler());
				handlers.AddMessageHandler("link", new LinkMsgHandler());

				var args = Response;
				handlers.HandleMessage(MSG);

				return "";
			}
			catch (Exception e)
			{
				Utities.Log(appPath + "weixinLog/msg_Error.txt", e.Message);
			}
			return "";
		}
	}
}
