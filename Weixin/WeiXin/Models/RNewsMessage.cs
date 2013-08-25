using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
	public class RNewsMessage : RMessage
	{
		public class NewsMeta
		{
			public string Title;
			public string Description;
			public string PicUrl;
			public string Url;

			public NewsMeta(string title, string descript, string picUrl, string url)
			{
				this.Title = title;
				this.Description = descript;
				this.PicUrl = picUrl;
				this.Url = url;
			}

			public NewsMeta(XmlNode node)
			{
				this.Title = node["Title"].InnerText;
				this.Description = node["Description"].InnerText;
				this.PicUrl = node["PicUrl"].InnerText;
				this.Url = node["Url"].InnerText;
			}

			public override string ToString()
			{
				string news = "<item>"
					+ "<Title><![CDATA[" + this.Title + "]]></Title>"
					+ "<Description><![CDATA[" + this.Description + "]]></Description>"
					+ "<PicUrl><![CDATA[" + this.PicUrl + "]]></PicUrl>"
					+ "<Url><![CDATA[" + this.Url + "]]></Url>"
					+ "</item>";

				return news;
			}

		}

		public List<NewsMeta> NewsItems = new List<NewsMeta>();

		public RNewsMessage()
			: base()
		{
		}
		public RNewsMessage(XmlDocument doc)
			: base(doc)
		{
			this.FromXmlDoc(doc);
		}
		public RNewsMessage(string from, string to, string newsXml = null, int funcflag = 0)
			: base("news", from, to)
		{
			if (!string.IsNullOrEmpty(newsXml))
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(newsXml);
				this.FromXmlDoc(doc);
			}
		}

		public override void FromXmlDoc(XmlDocument doc)
		{
			if (this.MsgType == "news")
			{
				XmlNodeList nodeList = doc.GetElementsByTagName("item");
				for (int i = 0; i < nodeList.Count; i++)
				{
					this.NewsItems.Add(new NewsMeta(nodeList[i]));
				}
			}
		}

		public void AddNews(string title, string descript, string picUrl, string url)
		{
			this.NewsItems.Add(new NewsMeta(title, descript, picUrl, url));
		}

		public void AddNews(NewsMeta news)
		{
			this.NewsItems.Add(news);
		}

		public override string ToString()
		{

			string msg = @"<xml>"
				+ "<ToUserName><![CDATA[" + this.To + "]]></ToUserName>"
				+ "<FromUserName><![CDATA[" + this.From + "]]></FromUserName>"
				+ "<CreateTime>" + this.Time.ToString() + "</CreateTime>"
				+ "<MsgType><![CDATA[news]]></MsgType>"
				+ "<ArticleCount>" + this.NewsItems.Count + "</ArticleCount>"
				+ "<Articles>";

			for (int i = 0; i < this.NewsItems.Count; i++)
			{
				msg += this.NewsItems[i].ToString();
			}

			msg += "</Articles>";
			msg += "<FuncFlag>" + this.FuncFlag + "</FuncFlag>";
			msg += "</xml>";

			return msg;
		}
	}
}
