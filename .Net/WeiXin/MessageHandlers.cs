using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.IO;

using WeiXin.Models;

namespace WeiXin
{
	//handled = true, stop the handlers list,
	//handled = false, continue to next handler
	public delegate RMessage MessageHandler(TMessage msg, ref bool handled);

	//只对推送的消息处理
	public class MessageHandlers
	{
		private Dictionary<string, List<MessageHandler>> MsgHandlers = new Dictionary<string, List<MessageHandler>>();

		//private TMessage Msg = null;
		//public RMessage RMsg = null;
		private System.IO.Stream OutStream;

		//public MessageHandlers(TMessage msg)
		//{
		//	Msg = msg;
		//}

		public MessageHandlers(System.IO.Stream stream)
		{
			this.OutStream = stream;
			MsgHandlers["text"] = new List<MessageHandler>();
			MsgHandlers["image"] = new List<MessageHandler>();
			MsgHandlers["location"] = new List<MessageHandler>();
			MsgHandlers["link"] = new List<MessageHandler>();
			MsgHandlers["event"] = new List<MessageHandler>();
			MsgHandlers["voice"] = new List<MessageHandler>();
		}

		public bool HandleMessage(TMessage msg)
		{
			string type = msg.MsgType;
			try
			{
				if (!object.Equals(null, MsgHandlers[type]))
				{
					List<MessageHandler> handlerList = MsgHandlers[type];
					for (int i = 0; i < handlerList.Count; i++)
					{
						MessageHandler handler = handlerList[i];
						bool handled = false;
						RMessage msg_r = handler(msg.GetMessage(), ref handled);
						if (handled)
						{
							StreamWriter sw = new StreamWriter(this.OutStream);
							sw.Write(msg_r.ToString());
							sw.Close();
							break;
						}
					}
					return true;
				}
			}
			catch (Exception e)
			{
				return false;
			}
			return false;
		}


		public bool AddMessageHandler(string type, MessageHandler handler)
		{
			try
			{
				if (!object.Equals(null, MsgHandlers[type]))
				{
					MsgHandlers[type].Add(handler);
					return true;
				}
			}
			catch (Exception e)
			{
				return false;
			}
			return false;
		}

		public bool AddMessageHandler(string type, IMessageHandler handler)
		{
			return this.AddMessageHandler(type, handler.Handler);
		}
	}
}
