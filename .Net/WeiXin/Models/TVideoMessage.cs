using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
    public class TVideoMessage : TMessage
    {
        public string MediaId = "";
        public string ThumbMediaId = "";
        public TVideoMessage()
            : base()
        {
        }
        public TVideoMessage(XmlDocument doc)
            : base(doc)
        {
            this.FromXmlDoc(doc);
        }
        public TVideoMessage(string id, string type, string from, string to, long time, XmlDocument doc = null)
            : base(id, type, from, to, time, doc)
        {
        }
        public override void FromXmlDoc(XmlDocument doc)
        {
            if (this.MsgType == "video")
            {
                this.MediaId = doc.GetElementsByTagName("MediaId")[0].InnerText;
                this.ThumbMediaId = doc.GetElementsByTagName("ThumbMediaId")[0].InnerText;
            }
        }
    }
}
