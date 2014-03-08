using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
    public class RVideoMessage : RMessage
    {
        public class VideoMeta
        {
            public string Title;
            public string Description;
            public string MediaId;

            public VideoMeta(string title, string descript, string mediaId)
            {
                this.Title = title;
                this.Description = descript;
                this.MediaId = mediaId;
            }
            public VideoMeta(XmlNode node)
            {
                this.Title = node["Title"].InnerText;
                this.Description = node["Description"].InnerText;
                this.MediaId = node["MediaId"].InnerText;
            }

            public override string ToString()
            {
                string video = @"<Video>"
                    + "<MediaId><![CDATA[" + this.MediaId + "]]></MediaId>"
                    + "<Title><![CDATA[" + this.Title + @"]]></Title>"
                    + "<Description><![CDATA[" + this.Description + "]]></Description>"
                    + "</Video>";
                return video;
            }

            public object ToJson()
            {
                var json = new
                {
                    media_id = this.MediaId,
                    title = this.Title,
                    description = this.Description
                };
                return json;
            }

        }

        public VideoMeta Video = null;

        public string Content = "";
        public RVideoMessage()
            : base()
        {
        }
        public RVideoMessage(XmlDocument doc)
            : base(doc)
        {
            this.FromXmlDoc(doc);
        }
        public RVideoMessage(string from, string to, string videoXml = null, int funcflag = 0)
            : base("video", from, to, funcflag)
        {
            if (!string.IsNullOrEmpty(videoXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(videoXml);
                this.FromXmlDoc(doc);
            }
        }

        public override void FromXmlDoc(XmlDocument doc)
        {
            if (this.MsgType == "video")
            {
                XmlNode node = doc.GetElementsByTagName("Video")[0];
                this.Video = new VideoMeta(node);
            }
        }


        public void SetVideo(string title, string descript, string mediaId)
        {
            this.Video = new VideoMeta(title, descript, mediaId);
        }

        public void SetVideo(VideoMeta video)
        {
            this.Video = video;
        }

        public override string ToString()
        {
            string msg = @"<xml>"
                + "<ToUserName><![CDATA[" + this.To + "]]></ToUserName>"
                + "<FromUserName><![CDATA[" + this.From + "]]></FromUserName>"
                + "<CreateTime>" + this.Time.ToString() + "</CreateTime>"
                + "<MsgType><![CDATA[video]]></MsgType>";

            msg += this.Video.ToString();

            msg += "<FuncFlag>" + this.FuncFlag + "</FuncFlag>";
            msg += "</xml>";

            return msg;
        }

        public override object ToJson()
        {
            var json = new
            {
                touser = this.To,
                msgtype = "video",
                video = this.Video.ToJson()
            };
            return json;
        }

    }
}
