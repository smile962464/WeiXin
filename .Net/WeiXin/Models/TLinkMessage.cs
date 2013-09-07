using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
	public class TLinkMessage : TMessage
	{
		public string Title = "";
		public string Description = "";
		public string Url = "";
		public TLinkMessage():base()
		{
		}
		public TLinkMessage(XmlDocument doc)
			: base(doc)
		{
			this.FromXmlDoc(doc);
		}
		public TLinkMessage(string id, string type, string from, string to, long time, XmlDocument doc = null)
			: base(id, type, from, to, time, doc)
		{
		}
		public override void FromXmlDoc(XmlDocument doc)
		{
			if (this.MsgType == "link")
			{
				this.Title = doc.GetElementsByTagName("Title")[0].InnerText;
				this.Description = doc.GetElementsByTagName("Description")[0].InnerText;
				this.Url = doc.GetElementsByTagName("Url")[0].InnerText;
			}
		}
	}
}
