using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace WeiXin
{
	public static class Utities
	{
		public static long GetTimeTicks(DateTime dt)
		{
			DateTime dt1 = new DateTime(1970, 1, 1);
			DateTime dt2 = new DateTime(1, 1, 1);

			TimeSpan sp = dt1 - dt2;

			DateTime ddt = dt - sp;

			long _time = ddt.Ticks / 10000000;

			return _time;
		}

		public static DateTime GetTime(long ticks)
		{
			DateTime dt = new DateTime(1970, 1, 1);

			dt.AddMilliseconds(ticks);

			return dt;
		}

		public static void Log(string logfile, string msg)
		{
			FileStream fs = new FileStream(logfile, FileMode.OpenOrCreate, FileAccess.Write);
			StreamWriter sr = new StreamWriter(fs);

			sr.WriteLine(msg);

			sr.Close();
			fs.Close();

		}

		//// Menu //////////
		public static string AccessToken(string appid = "wx8980382cb43a6897", string appsecret = "1137740a67c5152a9cd7af37abb4cd28")
		{
			string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + appsecret;
			using (var wc = new System.Net.WebClient())
			{
				var data = wc.DownloadData(url);
				string accessToken = Utities.GetStringFromBytes(data);
				return accessToken;
			}
		}
		
		public static string AddMenuFromFile(string accessToken, string path)
		{
			System.IO.FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			StreamReader sr = new StreamReader(fs);
			var menu = sr.ReadToEnd();
			string status = AddMenu(accessToken, menu);
			sr.Close();
			fs.Close();
			return status;
		}

		public static string AddMenu(string accessToken, string menu)
		{
			using (var wc = new System.Net.WebClient())
			{
				var data = Utities.GetBytesFromString(menu);
				var url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + accessToken;
				var res = wc.UploadData(url, data);
				return Utities.GetStringFromBytes(res);
			}
		}

		public static string GetMenu(string accessToken)
		{
			using (var wc = new System.Net.WebClient())
			{
				var url = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + accessToken;
				var res = wc.DownloadData(url);
				return Utities.GetStringFromBytes(res);
			}
		}

		public static string DeleteMenu(string accessToken)
		{
			using (var wc = new System.Net.WebClient())
			{
				var url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + accessToken;
				var res = wc.DownloadData(url);
				return Utities.GetStringFromBytes(res);
			}
		}

		public static string GetStringFromBytes(byte[] data)
		{
			return System.Text.Encoding.UTF8.GetString(data);
		}
		public static byte[] GetBytesFromString(string data)
		{
			byte[] _data = System.Text.Encoding.UTF8.GetBytes(data);
			return _data;
		}


	}
}
