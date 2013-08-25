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
	public class LocationMsgHandler : IMessageHandler
	{
		public RMessage Handler(TMessage inMsg, ref bool handled)
		{

			TLocationMessage location = (TLocationMessage)inMsg;

			string replayContent = "您的位置录入于" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "请在5分钟内上传图片！谢谢您提供的宝贵信息！";

			string sql = "insert into WeiXinLoc(wxusr,lat,lon,scale,type,inserttime) values('" + location.From + "'," + location.Lo_X.ToString()
				+ "," + location.Lo_Y.ToString() + "," + location.Scale.ToString() + ",1,getdate())";

			int rows = DbHelperSql.ExecuteSql(sql);

			RTextMessage msg_r = new RTextMessage(inMsg.To, inMsg.From, replayContent);

			handled = true;

			return msg_r;
		}
	}
}