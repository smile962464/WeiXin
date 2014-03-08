using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
    public class TVoiceMessage : TMessage
    {
        public string MediaId = "";
        public string Format = "";
        public string Recognition = "";
        public TVoiceMessage()
            : base()
        {
        }
        public TVoiceMessage(XmlDocument doc)
            : base(doc)
        {
            this.FromXmlDoc(doc);
        }
        public TVoiceMessage(string id, string type, string from, string to, long time, XmlDocument doc = null)
            : base(id, type, from, to, time, doc)
        {
        }
        public override void FromXmlDoc(XmlDocument doc)
        {
            if (this.MsgType == "voice")
            {
                this.MediaId = doc.GetElementsByTagName("MediaId")[0].InnerText;
                this.Format = doc.GetElementsByTagName("Format")[0].InnerText;
                var kl = doc.GetElementsByTagName("Recognition");
                if (kl.Count > 0)
                {
                    this.Recognition = kl[0].InnerText;
                }
            }
        }
    }
}
