using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WeiXin.Models;

namespace WeiXin
{
	public interface IMessageHandler
	{
		//MessageHandler Handler;
		RMessage Handler(TMessage inMsg, ref bool handled /*ref RMessage outMsg, object args = null*/);
	}
}
