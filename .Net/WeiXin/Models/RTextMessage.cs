using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
	public class RTextMessage : RMessage
	{
		public string Content = "";
		public RTextMessage()
			: base()
		{
		}
		public RTextMessage(XmlDocument doc)
			: base(doc)
		{
			this.FromXmlDoc(doc);
		}
		public RTextMessage(string from, string to, string content = "")
			: base("text", from, to)
		{
			this.Content = content;
		}

		public override void FromXmlDoc(XmlDocument doc)
		{
			if (this.MsgType == "text")
			{
				this.Content = doc.GetElementsByTagName("Content")[0].InnerText;
			}
		}


		public void SetContent(string content)
		{
			this.Content = content;
		}

		public override string ToString()
		{
			string msg = @"<xml>"
				+ "<ToUserName><![CDATA[" + this.To + "]]></ToUserName>"
				+ "<FromUserName><![CDATA[" + this.From + "]]></FromUserName>"
				+ "<CreateTime>" + this.Time.ToString() + "</CreateTime>"
				+ "<MsgType><![CDATA[text]]></MsgType>"
				+ "<Content><![CDATA[" + this.Content + "]]></Content>"
				+ "<FuncFlag>" + this.FuncFlag + "</FuncFlag>"
				+ "</xml>";

			return msg;
		}
	}
}
