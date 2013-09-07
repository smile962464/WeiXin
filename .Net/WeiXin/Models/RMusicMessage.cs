using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
	public class RMusicMessage : RMessage
	{
		public class MusicMeta
		{
			public string Title;
			public string Description;
			public string MusicUrl;
			public string HQMusicUrl;

			public MusicMeta(string title, string descript, string musicUrl, string hqmusicUrl)
			{
				this.Title = title;
				this.Description = descript;
				this.MusicUrl = musicUrl;
				this.HQMusicUrl = hqmusicUrl;
			}
			public MusicMeta(XmlNode node)
			{
				this.Title = node["Title"].InnerText;
				this.Description = node["Description"].InnerText;
				this.MusicUrl = node["MusicUrl"].InnerText;
				this.HQMusicUrl = node["HQMusicUrl"].InnerText;
			}

			public override string ToString()
			{

				string music = @"<Music>"
					+ "<Title><![CDATA[" + this.Title + @"]]></Title>"
					+ "<Description><![CDATA[" + this.Description + "]]></Description>"
					+ "<MusicUrl><![CDATA[" + this.MusicUrl + "]]></MusicUrl>"
					+ "<HQMusicUrl><![CDATA[" + this.HQMusicUrl + "]]></HQMusicUrl>"
					+ "</Music>";
				return music;
			}
		}

		public MusicMeta Music = null;

		public string Content = "";
		public RMusicMessage()
			: base()
		{
		}
		public RMusicMessage(XmlDocument doc)
			: base(doc)
		{
			this.FromXmlDoc(doc);
		}
		public RMusicMessage(string from, string to, string musicXml = null, int funcflag = 0)
			: base("music", from, to, funcflag)
		{
			if (!string.IsNullOrEmpty(musicXml))
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(musicXml);
				this.FromXmlDoc(doc);
			}
		}

		public override void FromXmlDoc(XmlDocument doc)
		{
			if (this.MsgType == "music")
			{
				XmlNode node = doc.GetElementsByTagName("Music")[0];
				this.Music = new MusicMeta(node);
			}
		}


		public void SetMusic(string title, string descript, string musicUrl, string hqmusicUrl)
		{
			this.Music = new MusicMeta(title, descript, musicUrl, hqmusicUrl);
		}

		public void SetMusic(MusicMeta music)
		{
			this.Music = music;
		}

		public override string ToString()
		{
			string msg = @"<xml>"
				+ "<ToUserName><![CDATA[" + this.To + "]]></ToUserName>"
				+ "<FromUserName><![CDATA[" + this.From + "]]></FromUserName>"
				+ "<CreateTime>" + this.Time.ToString() + "</CreateTime>"
				+ "<MsgType><![CDATA[music]]></MsgType>";

			msg += this.Music.ToString();

			msg += "<FuncFlag>" + this.FuncFlag + "</FuncFlag>";
			msg += "</xml>";

			return msg;
		}
	}
}
