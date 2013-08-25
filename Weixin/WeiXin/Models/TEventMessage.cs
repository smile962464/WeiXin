using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
	public class TEventMessage : TMessage
	{
		public string Event = "";
		public string EventKey = "";
		public TEventMessage()
			: base()
		{
		}
		public TEventMessage(XmlDocument doc)
			: base(doc)
		{
			this.FromXmlDoc(doc);
		}
		public TEventMessage(string id, string type, string from, string to, long time, XmlDocument doc = null)
			: base(id, type, from, to, time, doc)
		{
		}

		public override void FromXmlDoc(XmlDocument doc)
		{
			if (this.MsgType == "event")
			{
				this.Event = doc.GetElementsByTagName("Event")[0].InnerText;
				this.EventKey = doc.GetElementsByTagName("EventKey")[0].InnerText;
			}
		}
	}
}
