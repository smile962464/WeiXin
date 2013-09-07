using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
	public class TTextMessage : TMessage
	{
		public string Content = "";
		public TTextMessage():base()
		{
		}
		public TTextMessage(XmlDocument doc)
			: base(doc)
		{
			this.FromXmlDoc(doc);
		}
		public TTextMessage(string id, string type, string from, string to, long time, XmlDocument doc = null)
			: base(id, type, from, to, time, doc)
		{
		}

		public override void FromXmlDoc(XmlDocument doc)
		{
			if (this.MsgType == "text")
			{
				this.Content = doc.GetElementsByTagName("Content")[0].InnerText;
			}
		}
	}
}
