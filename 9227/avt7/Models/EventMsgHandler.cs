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
	public class EventMsgHandler : IMessageHandler
	{
		public RMessage Handler(TMessage inMsg, ref bool handled)
		{
			Dictionary<string, string> eventMap = new Dictionary<string, string>
			{
				{"METAR_FILTER_THUNDER","雷雨"},
				{"METAR_FILTER_FOG","大雾"},
				{"METAR_FILTER_COULD","低云"},
				{"METAR_FILTER_WIND","大风"}
			};

			TEventMessage evMsg = (TEventMessage)inMsg;

			handled = true;
			if (evMsg.Event == "CLICK")
			{
				TTextMessage txtMsg = new TTextMessage();
				txtMsg.From = evMsg.From;
				txtMsg.To = evMsg.To;
				txtMsg.MsgId = evMsg.MsgId;
				txtMsg.MsgType = "text";

				switch (evMsg.EventKey)
				{
					case "DYNAMIC_FLIGHT":
					case "DYNAMIC_METAR":
						string replyContent = "";
						if (evMsg.EventKey == "DYNAMIC_FLIGHT")
						{
							replyContent = "回复航班号查询航班时刻及相关机场天气实况，如MF8218，8218";
						}
						if (evMsg.EventKey == "DYNAMIC_METAR")
						{
							replyContent = "回复机场三字码、四字码或机场名称（例：XMN、ZSAM、虹桥）查询机场最新实况及预报报文";
						}

						RTextMessage msg_r = new RTextMessage(inMsg.To, inMsg.From, replyContent);

						return msg_r;
					case "WEATHER_INFO_NEWS":
						txtMsg.Content = "最新天气";
						return new TextMsgHandler().Handler(txtMsg, ref handled);
					//break;

					case "WEATHER_INFO_TYPHONE":
						txtMsg.Content = "台风信息";
						return new TextMsgHandler().Handler(txtMsg, ref handled);
					//break;

					case "METAR_FILTER_THUNDER":
					case "METAR_FILTER_FOG":
					case "METAR_FILTER_COULD":
					case "METAR_FILTER_WIND":

						txtMsg.Content = eventMap[evMsg.EventKey];
						return new TextMsgHandler().Handler(txtMsg, ref handled);
					//break;
				}
				return new RTextMessage(inMsg.To, inMsg.From, "未处理事件类型");
				//return true;
			}
			else
			{
				string replyContent = "";
				if (evMsg.Event == "subscribe")
				{

					//replyContent = "{\"menu\":{\"button\":[{\"type\":\"click\",\"name\":\"今日歌曲\",\"key\":\"V1001_TODAY_MUSIC\",\"sub_button\":[]},{\"type\":\"click\",\"name\":\"歌手简介\",\"key\":\"V1001_TODAY_SINGER\",\"sub_button\":[]},{\"name\":\"菜单\",\"sub_button\":[{\"type\":\"click\",\"name\":\"hello word\",\"key\":\"V1001_HELLO_WORLD\",\"sub_button\":[]},{\"type\":\"click\",\"name\":\"赞一下我们\",\"key\":\"V1001_GOOD\",\"sub_button\":[]}]}]}}";

					replyContent = "欢迎来到小飞象微信平台\n";
					replyContent += "查询方法：\n★发送航班号：航班时刻及相关机场天气实况；\n"
							+ "★发送机场三字码或四字码：机场最新实况及预报报文；\n★发送机场名称：机场最新实况；\n"
							+ "★发送“雷雨”或“1”：实况出现雷雨的机场；\n★“大雾”或“2”：实况出现大雾的机场；\n★“低云”或“3”：实况出现低云的机场；\n★“大风”或“4”：实况出现大风的机场；\n"
							+ "★发送“最新天气”或“5”：查询最新天气形势和机场预警。\n★发送“+工号+姓名+岗位”可加入认证用户。\n★发送“？”查询帮助信息。";

					string sql = "insert into WeiXinUsr(username) values('" + evMsg.From + "')";
					int exrows = DbHelperSql.ExecuteSql(sql);

				}
				else if (evMsg.Event == "unsubscribe")
				{
					string sql = "delete from WeiXinUsr where username='" + evMsg.From + "'";
					int exrows = DbHelperSql.ExecuteSql(sql);
					replyContent = "再见，欢迎再次关注小飞象！";
				}

				RTextMessage msg_r = new RTextMessage(inMsg.To, inMsg.From, replyContent);

				return msg_r;
			}
			//return false;
		}
	}
}