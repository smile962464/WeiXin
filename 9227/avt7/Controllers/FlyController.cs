using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;

using System.Data.OracleClient;

using WeiXin;
using WeiXin.Models;

using System.IO;
using System.Xml;

using avt7.Models;

using XiamenAir.DBTools;
using RegExp = System.Text.RegularExpressions;

namespace avt7.Controllers
{
	public class FlyTextMsgHandler : IMessageHandler
	{
		public RMessage Handler(TMessage inMsg, ref bool handled)
		{
			TTextMessage m = (TTextMessage)inMsg;

			var replayContent = this.GetAirlines(m.Content);
			RTextMessage msg_r = new RTextMessage(inMsg.To, inMsg.From, replayContent);

			handled = true;

			return msg_r;
		}

		public string GetAirlines(string message)
		{
			DateTime flightdate = DateTime.Now;
			string str_content = "";
			try
			{
				string[] str = message.Split('/');
				if (str[0].ToString() == null || str[0].ToString() == string.Empty || str[1].ToString() == string.Empty || str[1].ToString() == string.Empty)
				{
					str_content = "输入有误或无该人航班信息  1.查询人员航班信息，请输入格式：1/工号 例如：1/01234 即查询工号为01234的航班信息" + "\r\t" + "2.查询航班信息，请输入格式: 2/航班号 例如:2/8321 即查询航班号为MF8321的信息";
				}
				else
				{

					if (str[0].ToString() == "1")
					{

						DataSet AirlineStewDS = GetDataFromOracle(flightdate, str[1].ToString());
						if (AirlineStewDS == null || AirlineStewDS.Tables[0].Rows.Count == 0)
						{
							str_content = "输入有误或无该人航班信息  1.查询人员航班信息，请输入格式：1/工号 例如：1/01234 即查询工号为01234的航班信息" + "\r\t" + "2.查询航班信息，请输入格式: 2/航班号 例如:2/8321 即查询航班号为MF8321的信息";
						}
						else
						{
							str_content = "";
							for (int i = 0; i < AirlineStewDS.Tables[0].Rows.Count; i++)
							{
								str_content += "//" + AirlineStewDS.Tables[0].Rows[i]["c_name"].ToString();
								str_content += " 航班日期: " + Convert.ToDateTime(AirlineStewDS.Tables[0].Rows[i]["flight_date"]).ToLongDateString();
								str_content += "  航班号: " + AirlineStewDS.Tables[0].Rows[i]["flight_com"].ToString();
								str_content += "  航程: " + AirlineStewDS.Tables[0].Rows[i]["flight_voyage"].ToString();
								str_content += "  起飞落地时刻: " + Convert.ToDateTime(AirlineStewDS.Tables[0].Rows[i]["td"]).ToLongTimeString();
								str_content += "--" + Convert.ToDateTime(AirlineStewDS.Tables[0].Rows[i]["ta"]).ToLongTimeString() + "\r\t";
							}
							str_content += "//\r\t";
						}
					}
					else
					{
						if (str[0].ToString() == "2")
						{
							DataSet AirlineStewDS = GetDataFromOracle2(str[1].ToString(), flightdate);
							if (AirlineStewDS == null || AirlineStewDS.Tables[0].Rows.Count == 0)
							{
								str_content = "输入有误或无该人航班信息  1.查询人员航班信息，请输入格式：1/工号 例如：1/01234 即查询工号为01234的航班信息" + "\r\t" + "2.查询航班信息，请输入格式: 2/航班号 例如:2/8321 即查询航班号为MF8321的信息";
							}
							else
							{
								str_content = "";
								for (int i = 0; i < AirlineStewDS.Tables[0].Rows.Count; i++)
								{
									str_content += "// 航班日期: " + Convert.ToDateTime(AirlineStewDS.Tables[0].Rows[i]["flight_date"]).ToLongDateString();
									str_content += "  起飞: " + AirlineStewDS.Tables[0].Rows[i]["departure_airport"].ToString();
									str_content += "  落地: " + AirlineStewDS.Tables[0].Rows[i]["arrival_airport"].ToString();
									str_content += "  航班号: " + AirlineStewDS.Tables[0].Rows[i]["flight_no"].ToString();
									str_content += "  机型: " + AirlineStewDS.Tables[0].Rows[i]["ac_type"].ToString();
									str_content += "  机号: " + AirlineStewDS.Tables[0].Rows[i]["ac_reg"].ToString();
									str_content += "  起飞落地时刻: " + Convert.ToDateTime(AirlineStewDS.Tables[0].Rows[i]["std"]).ToLongTimeString();
									str_content += "--" + Convert.ToDateTime(AirlineStewDS.Tables[0].Rows[i]["sta"]).ToLongTimeString() + "\r\t";
								}
								str_content += "//\r\t";
							}
						}
					}


				}


			}
			catch
			{
				str_content = "输入有误或无该人航班信息  1.查询人员航班信息，请输入格式：1/工号 例如：1/01234 即查询工号为01234的航班信息" + "\r\t" + "2.查询航班信息，请输入格式: 2/航班号 例如:2/8321 即查询航班号为MF8321的信息";
				return str_content;
			}
			return str_content;
		}

		/// <summary>
		/// 获取航班信息
		/// </summary>
		/// <param name="flightdate"></param>
		/// <param name="p_code"></param>
		/// <returns></returns>
		public DataSet GetDataFromOracle(DateTime flightdate, string worker_no)
		{
			string contr = "Data Source=foc10g;User ID=mffoc;Password=m1dkundidko;Unicode=True";
			OracleConnection oraclCon = new OracleConnection();
			oraclCon.ConnectionString = contr;
			OracleCommand oracleCom = oraclCon.CreateCommand();
			try
			{
				oraclCon.Open();
				string time = flightdate.Date.ToShortDateString();
				oracleCom.CommandText = "select h.c_name,w.flight_date,t.flight_voyage,t.flight_com,t.td,t.ta from t3013 t,t3016 w,t3017 h where w.flight_date >= to_date('" + time + "','YYYY-MM-DD') and w.flight_date = t.flight_date and w.stew_link_line = t.stew_link_line and w.p_code = h.p_code and  h.worker_no ='" + worker_no + "'";
				OracleDataAdapter ODA = new OracleDataAdapter(oracleCom);
				DataSet ds = new DataSet();
				ODA.Fill(ds);
				oraclCon.Close();
				if (ds.Tables[0].Rows.Count > 0)
				{
					return ds;
				}
				else
				{
					return null;
				}

			}
			catch (Exception e)
			{

				string str = e.Message;
				return null;
			}

		}


		public DataSet GetDataFromOracle2(string fight_no, DateTime flightdate)
		{
			string contr = "Data Source=foc10g;User ID=mffoc;Password=m1dkundidko;Unicode=True";
			OracleConnection oraclCon = new OracleConnection();
			oraclCon.ConnectionString = contr;
			OracleCommand oracleCom = oraclCon.CreateCommand();
			try
			{
				oraclCon.Open();
				string time = flightdate.Date.ToShortDateString();
				fight_no = "MF" + fight_no;
				oracleCom.CommandText = "select w.flight_date,w.departure_airport,w.arrival_airport,w.flight_no,w.ac_reg,w.ac_type,w.std,w.sta from t2001 w where w.flight_date = to_date('" + time + "','YYYY-MM-DD') and w.flight_no = '" + fight_no + "'";
				OracleDataAdapter ODA = new OracleDataAdapter(oracleCom);
				DataSet ds = new DataSet();
				ODA.Fill(ds);
				oraclCon.Close();
				if (ds.Tables[0].Rows.Count > 0)
				{
					return ds;
				}
				else
				{
					return null;
				}

			}
			catch (Exception e)
			{

				string str = e.Message;
				return null;
			}

		}
	}
	public class FlyController : Controller
	{
		//
		// GET: /Fly/

		[HttpGet]
		public string Index(string signature, string timestamp, string nonce, string echostr)
		{
			//Utities.Log("D:/weixinLog/sig_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".txt", echostr);
			return echostr;
		}

		//Post : /Fly/
		[HttpPost]
		[ValidateInput(false)]
		public string Index()
		{
			try
			{
				Stream input = Request.InputStream;

				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(Request.InputStream);
				Utities.Log("D:/weixinLog/msg_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".txt", xmlDoc.InnerXml);

				TMessage MSG = new TMessage(xmlDoc);

				MessageHandlers handlers = new MessageHandlers(this.Response.OutputStream);
				handlers.AddMessageHandler("text", new FlyTextMsgHandler());
				handlers.AddMessageHandler("link", this.LinkMsgHandler);
				handlers.AddMessageHandler("link", this.LinkMsgHandler2);

				var args = Response;
				handlers.HandleMessage(MSG);

				return "";
			}
			catch (Exception e)
			{
				Utities.Log("D:/weixinLog/msg_Error.txt", e.Message);
			}
			return "";
		}


		public RMessage LinkMsgHandler(TMessage inMsg, ref bool handled)
		{
			TLinkMessage m = (TLinkMessage)inMsg;

			var replayContent = "This is a Linked Message";
			RTextMessage msg_r = new RTextMessage(inMsg.To, inMsg.From, replayContent);

			if (m.Url != "url")
			{
				handled = true;
			}

			return msg_r;
		}
		public RMessage LinkMsgHandler2(TMessage inMsg, ref bool handled)
		{
			TLinkMessage m = (TLinkMessage)inMsg;

			var replayContent = "This is another Linked Message";
			RTextMessage msg_r = new RTextMessage(inMsg.To, inMsg.From, replayContent);

			handled = true;

			return msg_r;
		}
	}
}
