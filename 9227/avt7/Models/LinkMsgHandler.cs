using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using XiamenAir.DBTools;

using RegExp = System.Text.RegularExpressions;
using System.Text.RegularExpressions;

using WeiXin;
using WeiXin.Models;

namespace avt7.Models
{
	public class LinkMsgHandler : IMessageHandler
	{
		public RMessage Handler(TMessage inMsg, ref bool handled)
		{
			TLinkMessage lMsg = (TLinkMessage)inMsg;

			///音乐消息测试
			RMusicMessage msg_m = new RMusicMessage(inMsg.To, inMsg.From);
			RMusicMessage.MusicMeta music = new RMusicMessage.MusicMeta("Music Message", "音乐消息测试", "http://mp.weixin.qq.com/cgi-bin/downloadfile?token=1995995487&fileid=10000022", "http://mp.weixin.qq.com/cgi-bin/downloadfile?token=1995995487&fileid=10000022");
			msg_m.SetMusic(music);

			handled = true;

			return msg_m;
		}
	}
}