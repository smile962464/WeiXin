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
	public class ImageMsgHandler : IMessageHandler
	{
		public RMessage Handler(TMessage inMsg, ref bool handled)
		{
			string replayContent = "";

			TImageMessage iMsg = (TImageMessage)inMsg;

			string sql = "select top 1 id from WeiXinLoc where wxusr='" + iMsg.From + "' and abs(datediff(MI,inserttime,getdate()))<5 order by inserttime desc";
			string msgid = DbHelperSql.ExecuteScaleString(sql);
			if (string.IsNullOrEmpty(msgid) == false)
			{
				sql = "insert into WeiXinImg(wxusr,imgurl,inserttime,locid) values('" + iMsg.From + "','" + iMsg.PicUrl + "',getdate()," + msgid + ")";
				int rows = DbHelperSql.ExecuteSql(sql);
				replayContent = "您的图片录入于" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "感谢您提供的宝贵信息！";
			}
			else
			{
				replayContent = "请先录入位置信息！";
			}
			///新闻消息测试
			//RNewsMessage msg_n = new RNewsMessage(msg.To, msg.From);
			//msg_n.AddNews("Image Messsage 1", "测试新闻消息-1", "http://www.xiamenair.com/cn/cn/UserFiles/admin/images/cn/original/s201302250846502080427802.jpg", "http://www.xiamenair.com.cn");
			//msg_n.AddNews("Image Messsage 2", "测试新闻消息-2", "http://www.xiamenair.com/cn/cn/UserFiles/admin/images/cn/original/s201302191639222080427802.jpg", "http://www.xiamenair.com.cn");
			//msg_n.AddNews("Image Messsage 3", "测试新闻消息-3", "http://www.xiamenair.com/cn/cn/UserFiles/admin/images/cn/original/s201304171515282080427802.jpg", "http://www.xiamenair.com.cn");
			//msg_n.AddNews("Image Messsage 4", "测试新闻消息-4", "http://www.xiamenair.com/cn/cn/UserFiles/admin/images/cn/original/s201304121034032080427802.jpg", "http://www.xiamenair.com.cn");

			RTextMessage msg_r = new RTextMessage(inMsg.To, inMsg.From, replayContent);

			handled = true;

			return msg_r;
		}
	}
}