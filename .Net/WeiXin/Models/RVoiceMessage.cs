using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
    public class RVoiceMessage : RMessage
    {
        public class VoiceMeta
        {
            public string MediaId;

            public VoiceMeta(string mediaId)
            {
                this.MediaId = mediaId;
            }
            public VoiceMeta(XmlNode node)
            {
                this.MediaId = node["MediaId"].InnerText;
            }

            public override string ToString()
            {
                string voice = @"<Voice>"
                    + "<MediaId><![CDATA[" + this.MediaId + "]]></MediaId>"
                    + "</Voice>";
                return voice;
            }

            public object ToJson()
            {
                var json = new
                {
                    media_id = this.MediaId
                };
                return json;
            }

        }

        public VoiceMeta Voice = null;

        public string Content = "";
        public RVoiceMessage()
            : base()
        {
        }
        public RVoiceMessage(XmlDocument doc)
            : base(doc)
        {
            this.FromXmlDoc(doc);
        }
        public RVoiceMessage(string from, string to, string voiceXml = null, int funcflag = 0)
            : base("voice", from, to, funcflag)
        {
            if (!string.IsNullOrEmpty(voiceXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(voiceXml);
                this.FromXmlDoc(doc);
            }
        }

        public override void FromXmlDoc(XmlDocument doc)
        {
            if (this.MsgType == "voice")
            {
                XmlNode node = doc.GetElementsByTagName("Voice")[0];
                this.Voice = new VoiceMeta(node);
            }
        }


        public void SetVoice(string mediaId)
        {
            this.Voice = new VoiceMeta(mediaId);
        }

        public void SetVoice(VoiceMeta voice)
        {
            this.Voice = voice;
        }

        public override string ToString()
        {
            string msg = @"<xml>"
                + "<ToUserName><![CDATA[" + this.To + "]]></ToUserName>"
                + "<FromUserName><![CDATA[" + this.From + "]]></FromUserName>"
                + "<CreateTime>" + this.Time.ToString() + "</CreateTime>"
                + "<MsgType><![CDATA[voice]]></MsgType>";

            msg += this.Voice.ToString();

            msg += "<FuncFlag>" + this.FuncFlag + "</FuncFlag>";
            msg += "</xml>";

            return msg;
        }

        public override object ToJson()
        {
            var json = new
            {
                touser = this.To,
                msgtype = "voice",
                voice = this.Voice.ToJson()
            };
            return json;
        }

    }
}
