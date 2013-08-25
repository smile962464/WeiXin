using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;

namespace WeiXin.Models
{
	public class TMessage : Message
	{
		public string MsgId = "";
		public XmlDocument xml = null;

		public TMessage()
		{
		}

		public TMessage(XmlDocument doc)
			: base(doc)
		{
			this.xml = doc;
			this.FromXmlDoc(doc);
		}

		public TMessage(string id, string type, string from, string to, long time, XmlDocument doc = null)
			: base(type, from, to, time)
		{
			this.MsgId = id;
			this.xml = doc;
		}

		public TMessage GetMessage(XmlDocument doc = null)
		{
			var xmlDoc = this.xml;
			if (!object.Equals(doc,null))
			{
				xmlDoc = doc;
			}
			switch(this.MsgType){
				case "text":
					TTextMessage t_msg = new TTextMessage(this.MsgId, this.MsgType, this.From, this.To, this.Time, this.xml);
					t_msg.FromXmlDoc(xmlDoc);
					return t_msg;
				case "image":
					TImageMessage p_msg = new TImageMessage(this.MsgId, this.MsgType, this.From, this.To, this.Time, this.xml);
					p_msg.FromXmlDoc(xmlDoc);
					return p_msg;
				case "link":
					TLinkMessage li_msg = new TLinkMessage(this.MsgId, this.MsgType, this.From, this.To, this.Time, this.xml);
					li_msg.FromXmlDoc(xmlDoc);
					return li_msg;
				case "location":
					TLocationMessage lo_msg = new TLocationMessage(this.MsgId, this.MsgType, this.From, this.To, this.Time, this.xml);
					lo_msg.FromXmlDoc(xmlDoc);
					return lo_msg;
				case "event":
					TEventMessage e_msg = new TEventMessage(this.MsgId, this.MsgType, this.From, this.To, this.Time, this.xml);
					e_msg.FromXmlDoc(xmlDoc);
					return e_msg;
			}
			return null;
		}

		public virtual void FromXmlDoc(XmlDocument doc) { }

	}
}
