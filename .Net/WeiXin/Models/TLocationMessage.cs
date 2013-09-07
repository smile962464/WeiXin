using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
	public class TLocationMessage : TMessage
	{
		public double Lo_X = 0.0;
		public double Lo_Y = 0.0;
		public double Scale = 0.0;
		public string Label = "";

		public TLocationMessage()
			: base()
		{
		}
		public TLocationMessage(XmlDocument doc)
			: base(doc)
		{
			this.FromXmlDoc(doc);
		}
		public TLocationMessage(string id, string type, string from, string to, long time, XmlDocument doc = null)
			: base(id, type, from, to, time, doc)
		{
			this.FromXmlDoc(doc);
		}
		public override void FromXmlDoc(XmlDocument doc)
		{
			if (this.MsgType == "location")
			{
				this.Lo_X = Convert.ToDouble(doc.GetElementsByTagName("Location_X")[0].InnerText);
				this.Lo_Y = Convert.ToDouble(doc.GetElementsByTagName("Location_Y")[0].InnerText);
				this.Scale = Convert.ToDouble(doc.GetElementsByTagName("Scale")[0].InnerText);
				this.Label = doc.GetElementsByTagName("Label")[0].InnerText;
			}
		}
	}
}
