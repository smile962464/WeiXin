using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
    public class RImageMessage : RMessage
    {
        public class ImageMeta
        {
            public string MediaId;

            public ImageMeta(string mediaId)
            {
                this.MediaId = mediaId;
            }
            public ImageMeta(XmlNode node)
            {
                this.MediaId = node["MediaId"].InnerText;
            }

            public override string ToString()
            {
                string image = @"<Image>"
                    + "<MediaId><![CDATA[" + this.MediaId + "]]></MediaId>"
                    + "</Image>";
                return image;
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

        public ImageMeta Image = null;

        public string Content = "";
        public RImageMessage()
            : base()
        {
        }
        public RImageMessage(XmlDocument doc)
            : base(doc)
        {
            this.FromXmlDoc(doc);
        }
        public RImageMessage(string from, string to, string imageXml = null, int funcflag = 0)
            : base("image", from, to, funcflag)
        {
            if (!string.IsNullOrEmpty(imageXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(imageXml);
                this.FromXmlDoc(doc);
            }
        }

        public override void FromXmlDoc(XmlDocument doc)
        {
            if (this.MsgType == "image")
            {
                XmlNode node = doc.GetElementsByTagName("Image")[0];
                this.Image = new ImageMeta(node);
            }
        }


        public void SetImage(string mediaId)
        {
            this.Image = new ImageMeta(mediaId);
        }

        public void SetImage(ImageMeta image)
        {
            this.Image = image;
        }

        public override string ToString()
        {
            string msg = @"<xml>"
                + "<ToUserName><![CDATA[" + this.To + "]]></ToUserName>"
                + "<FromUserName><![CDATA[" + this.From + "]]></FromUserName>"
                + "<CreateTime>" + this.Time.ToString() + "</CreateTime>"
                + "<MsgType><![CDATA[image]]></MsgType>";

            msg += this.Image.ToString();

            msg += "<FuncFlag>" + this.FuncFlag + "</FuncFlag>";
            msg += "</xml>";

            return msg;
        }

        public override object ToJson()
        {
            var json = new
            {
                touser = this.To,
                msgtype = "image",
                image = this.Image.ToJson()
            };
            return json;
        }
    }
}
