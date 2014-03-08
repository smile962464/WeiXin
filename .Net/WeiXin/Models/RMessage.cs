using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;

namespace WeiXin.Models
{
    public class RMessage : Message
    {
        //默认不星标消息
        public int FuncFlag = 0;
        public XmlDocument xml = null;

        public RMessage()
        {
        }

        public RMessage(XmlDocument doc)
            : base(doc)
        {
            this.FuncFlag = Convert.ToInt32(doc.GetElementsByTagName("FuncFlag")[0].InnerText);

        }

        public RMessage(string type, string from, string to, int funcflag = 0)
            : base(type, from, to, Utities.GetTimeTicks(DateTime.Now))
        {
            this.FuncFlag = funcflag;
        }

        public virtual void FromXmlDoc(XmlDocument doc) { }


        public virtual object ToJson()
        {
            return null;
        }

        public virtual string ToJsonString()
        {
            return Utities.JsonSerialize(this.ToJson());
        }

    }
}
