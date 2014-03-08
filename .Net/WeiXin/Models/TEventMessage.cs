using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace WeiXin.Models
{
    public class TEventMessage : TMessage
    {
        public string Event = "";
        public Dictionary<string, string> EventData = new Dictionary<string, string>();
        public TEventMessage()
            : base()
        {
        }
        public TEventMessage(XmlDocument doc)
            : base(doc)
        {
            this.FromXmlDoc(doc);
        }
        public TEventMessage(string id, string type, string from, string to, long time, XmlDocument doc = null)
            : base(id, type, from, to, time, doc)
        {
        }

        public string GetEventData(string key)
        {
            if (EventData.Keys.Contains(key))
                return this.EventData[key];
            return null;
        }
        public override void FromXmlDoc(XmlDocument doc)
        {
            if (this.MsgType == "event")
            {
                this.Event = doc.GetElementsByTagName("Event")[0].InnerText;

                string[] dataList = new string[] { "EventKey", "Ticket", "Latitude", "Longitude", "Precision" };

                foreach (var key in dataList)
                {
                    var keyL = doc.GetElementsByTagName(key);
                    if (keyL.Count > 0)
                    {
                        this.EventData[key] = keyL[0].InnerText;
                    }
                }
            }
        }
    }
}
