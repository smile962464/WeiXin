using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;

using WeiXin;

namespace WeiXin.Models
{
	public class Message
	{
		public string To = "";
		public string From = "";

		public string MsgType = "";

		//Time Ticks with the same meaning of WeiXin PlatForm
		public long Time = 0;
		public DateTime _Time = DateTime.Now;

		public Message()
		{
		}

		public Message(XmlDocument doc)
		{
			this.To = doc.GetElementsByTagName("ToUserName")[0].InnerText;
			this.From = doc.GetElementsByTagName("FromUserName")[0].InnerText;
			this.Time = Convert.ToInt32(doc.GetElementsByTagName("CreateTime")[0].InnerText);
			this.MsgType = doc.GetElementsByTagName("MsgType")[0].InnerText;
			this._Time = Utities.GetTime(this.Time);
		}

		public Message(string type, string from, string to, long time)
		{
			this.MsgType = type;
			this.From = from;
			this.To = to;
			this.Time = time;
			this._Time = Utities.GetTime(this.Time);
		}

		public void SetTime(DateTime dt)
		{
			this._Time = dt;
			this.Time = Utities.GetTimeTicks(dt);
		}
	}
}
