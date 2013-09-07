using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
	public class TImageMessage : TMessage
	{
		public string PicUrl = "";
		public TImageMessage():base()
		{
		}
		public TImageMessage(XmlDocument doc)
			: base(doc)
		{
			this.FromXmlDoc(doc);
		}
		public TImageMessage(string id, string type, string from, string to, long time, XmlDocument doc = null)
			: base(id, type, from, to, time, doc)
		{
		}
		public override void FromXmlDoc(XmlDocument doc)
		{
			if (this.MsgType == "image")
			{
				this.PicUrl = doc.GetElementsByTagName("PicUrl")[0].InnerText;
			}
		}
	}
}
