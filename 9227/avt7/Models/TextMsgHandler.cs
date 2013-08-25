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
	public class TextMsgHandler : IMessageHandler
	{
		public RMessage Handler(TMessage inMsg, ref bool handled)
		{
//			string ap_sql = @"select t.airport_4code, t.airport_3code, t.chinese_name, t.city_ch_name from xhfoc.t7001 t
//				where t.a_gzt = 'Y'";
//			DataSet ap_ds = DbHelperOra.Execute(ap_sql);
//			var ap_query = ap_ds.Tables[0].AsEnumerable();

			handled = true;

			string replayContent = "";
			TTextMessage WMsg = (TTextMessage)inMsg;//.GetMessage();

			string content = WMsg.Content;

			string sql = "";

			string isInternal = "N";
			//此处一定要捕获异常，防止数据查询失败，导致后面代码无法执行！！！
			//用户认证
			try
			{
				sql = "select msgswitch from WeiXinUsr where username='" + inMsg.From + "'";
				isInternal = DbHelperSql.ExecuteScaleString(sql);       // "Y"为认证用户
			}
			catch (Exception e)
			{
			}
			//机号匹配
			Regex flyPatt = new Regex("^[0-9]{4}", RegexOptions.IgnoreCase);
			//航班号匹配
			Regex fltPatt = new Regex("^MF[38][0-9]{2,3}|^[38][0-9]{2,3}", RegexOptions.IgnoreCase);
			//机场匹配
			Regex apPatt = new Regex("^[a-zA-Z]{3,4}", RegexOptions.IgnoreCase);
			//内部用户信息+工号+姓名+岗位
			Regex usrPatt = new Regex(@"^[+]\d+[+].+[+].+", RegexOptions.IgnoreCase);

			Regex usrPatt2 = new Regex(@"^[＋]\d+[＋].+[＋].+", RegexOptions.IgnoreCase);

			Regex airpPatt = new Regex(@"(^\w{4}\w)", RegexOptions.IgnoreCase);      //气候查询机场提醒查询

			//Regex hintPatt = new Regex(@"(^\w{4}hint)", RegexOptions.IgnoreCase);              

			Match flyMa = flyPatt.Match(content);
			Match fltMa = fltPatt.Match(content);
			Match apMa = apPatt.Match(content);
			Match usrMa = usrPatt.Match(content);
			Match usrMa2 = usrPatt2.Match(content);
			Match airpMa = airpPatt.Match(content);


			if (fltMa.Success)
			{
				//
				string fltNo = content.ToUpper();
				//xxxx --> MFxxxx
				if (fltNo.ElementAt(0) != 'M')
				{
					fltNo = "MF" + fltNo;
				}
				//查询航班
				string flt_sql = @"select rownum, t.flight_id, t.flight_date, t.flight_no,t.close_door_time closetime,t.section, t.ac_reg,
					nvl(To_Char(t.etd, 'HH24:MI'),To_Char(t.std, 'HH24:MI')) etd, To_Char(t.atd, 'HH24:MI') atd, 
					nvl(To_Char(t.eta, 'HH24:MI'),To_Char(t.sta, 'HH24:MI')) eta, To_Char(t.ata, 'HH24:MI') ata,
					t.departure_airport, t1.city_3code Departure3Code, t1.chinese_name departure, t1.city_ch_name departure_city, 
					t.arrival_airport,   t2.city_3code Arrival3Code,   t2.chinese_name arrival,   t2.city_ch_name arrival_city
					from xhfoc.t2001 t, xhfoc.t7001 t1, xhfoc.t7001 t2  
					where t.flight_date = trunc(sysdate) 
					and t.carrier = 'MF'
					and t1.airport_4code = t.departure_airport
					and t2.airport_4code = t.arrival_airport  
					and ((t.ADJUST_TYPE <> '0' and t.ADJUST_TYPE <> 'b') or (t.ADJUST_TYPE is null))
					and t.flight_no ='" + fltNo + @"'
					order by t.section";

				DataSet flt_ds = DbHelperOra.Execute(flt_sql);
				if (flt_ds.Tables.Count > 0 && flt_ds.Tables[0].Rows.Count > 0)
				{
					DataRowCollection rows = flt_ds.Tables[0].Rows;
					//联程航班
					if (rows.Count > 1)
					{
						DataRow row_s = rows[0];
						DataRow row_e = rows[rows.Count - 1];
						replayContent += row_s["flight_no"].ToString();
						if (isInternal == "Y")
						{
							replayContent += "航班由" + row_s["ac_reg"].ToString() + "执行，自" + row_s["departure_city"].ToString() + row_s["departure"].ToString() + "机场经停";
						}
						else
						{
							replayContent += "航班由" + row_s["departure_city"].ToString() + row_s["departure"].ToString() + "机场经停";
						}
						for (int i = 0; i < rows.Count - 1; i++)
						{
							var row = rows[i];
							replayContent += row["arrival_city"].ToString() + row["arrival"].ToString() + "机场，";
						}
						replayContent += "飞往" + row_e["arrival_city"].ToString() + row_e["arrival"].ToString() + "机场；";

					}
					else //单程航班
					{
						DataRow row = rows[0];
						replayContent += row["flight_no"].ToString();
						if (isInternal == "Y")
						{
							replayContent += "航班由" + row["ac_reg"].ToString() + "执行，自" + row["departure_city"].ToString() + row["departure"].ToString() + "机场飞往";
						}
						else
						{
							replayContent += "航班由" + row["departure_city"].ToString() + row["departure"].ToString() + "机场飞往";
						}

						replayContent += row["arrival_city"].ToString() + row["arrival"].ToString() + "机场；";
					}
					DataRow row_d = rows[0];
					DataRow row_a = rows[rows.Count - 1];
					if (string.IsNullOrEmpty(row_d["atd"].ToString()))
					{
						replayContent += "航班";
						if (string.IsNullOrEmpty(row_d["closetime"].ToString()) == false)
						{
							replayContent += "已于" + row_d["closetime"].ToString() + "关舱，";
						}
						replayContent += "预计将于" + row_d["etd"].ToString() + "出发，";
					}
					else
					{
						if (string.IsNullOrEmpty(row_d["ata"].ToString()))
						{
							replayContent += "航班于" + row_d["atd"].ToString() + "出发，";
							replayContent += "预计" + row_a["eta"].ToString() + "到达" + row_a["arrival_city"].ToString() + row_a["arrival"].ToString() + "机场。";

						}
						else
						{
							if (string.IsNullOrEmpty(row_a["atd"].ToString()))
							{
								replayContent += "航班";
								if (string.IsNullOrEmpty(row_a["closetime"].ToString()) == false)
								{
									replayContent += "已于" + row_a["closetime"].ToString() + "关舱，";
								}
								replayContent += "预计将于" + row_a["etd"].ToString() + "出发，";
							}
							else
							{
								replayContent += "航班于" + row_a["atd"].ToString() + "出发，";
								if (string.IsNullOrEmpty(row_a["ata"].ToString()))
								{
									replayContent += "预计" + row_a["eta"].ToString() + "到达" + row_a["arrival_city"].ToString() + row_a["arrival"].ToString() + "机场。";
								}
								else
								{
									replayContent += "已于" + row_a["ata"].ToString() + "到达" + row_a["arrival_city"].ToString() + row_a["arrival"].ToString() + "机场。";
								}
							}
						}
					}


					replayContent += "\n\n";
					replayContent += "途径城市天气实况:\n";
					DataRow _r = rows[0];
					string city;
					for (int i = 0; i < rows.Count; i++)
					{
						_r = rows[i];
						city = _r["departure_city"].ToString();
						//replayContent += city + ":" + queryCity(city) + ";\n";
						replayContent += queryCity(city) + ";\n";
					}
					city = _r["arrival_city"].ToString();
					replayContent += queryCity(city) + "。";

				}
				else
				{
					replayContent = "无此航班，请确认航班号";
				}

			}
			else if (flyMa.Success)
			{
				replayContent = "机号";
				//
			}
			else if (usrMa.Success || usrMa2.Success)
			{
				//内部用户信息:+工号+姓名+岗位
				string[] usrInfo = null;
				sql = "select * from WeiXinUsr where username='" + inMsg.From + "'";
				string usr = DbHelperSql.ExecuteScaleString(sql);
				if (usrMa.Success)
				{
					usrInfo = content.Split('+');
				}
				else if (usrMa2.Success)
				{
					usrInfo = content.Split('＋');
				}
				if (string.IsNullOrEmpty(usr))
				{

					sql = "select dep from WeiXinInternalUsr where worknum=" + usrInfo[1] + " and name='" + usrInfo[2] + "'";
					string dep = DbHelperSql.ExecuteScaleString(sql);
					if (string.IsNullOrEmpty(dep))
					{
						sql = "insert into WeiXinUsr(worknum,username,realname,department) values(" + usrInfo[1] + ",'" + inMsg.From + "','"
						+ usrInfo[2] + "','" + usrInfo[3] + "')";
						int exrows = DbHelperSql.ExecuteSql(sql);
						replayContent = "您好" + usrInfo[2] + ",欢迎加入小飞象航空气象服务,请输入正确的工号和姓名（OA中名字带数字的需加数字）！";
					}
					else
					{
						sql = "insert into WeiXinUsr(worknum,username,realname,department,msgswitch,addtime) values(" + usrInfo[1] + ",'" + inMsg.From + "','"
						+ usrInfo[2] + "','" + usrInfo[3] + "','Y',getdate())";
						int exrows = DbHelperSql.ExecuteSql(sql);
						replayContent = "您好,来自" + dep + "的" + usrInfo[2] + ",欢迎加入小飞象航空气象服务\n★输入四字码1，例如zsam1查询机场气候；\n★输入四字码2，例如zsam2查询机场提醒；\n我们将为您提供更专业的服务！";
					}
				}
				else
				{
					sql = "select dep from WeiXinInternalUsr where worknum=" + usrInfo[1] + " and name='" + usrInfo[2] + "'";
					string dep = DbHelperSql.ExecuteScaleString(sql);
					if (string.IsNullOrEmpty(dep))
					{
						sql = "update WeiXinUsr set worknum=" + usrInfo[1] + ",realname='"
						+ usrInfo[2] + "',department='" + usrInfo[3] + "',msgswitch='N' where username='" + inMsg.From + "'";
						int exrows = DbHelperSql.ExecuteSql(sql);
						replayContent = "已将您的信息更新到数据库，请确认输入正确的工号和姓名（OA中名字带数字的需加数字），谢谢！";
					}
					else
					{
						sql = "update WeiXinUsr set worknum=" + usrInfo[1] + ",realname='"
						+ usrInfo[2] + "',department='" + usrInfo[3] + "',msgswitch='Y' where username='" + inMsg.From + "'";
						int exrows = DbHelperSql.ExecuteSql(sql);
						replayContent = "来自" + dep + "的已将您的信息更新到数据库\n★输入四字码1，例如zsam1查询机场气候；\n★输入四字码2，例如zsam2查询机场提醒；\n我们将为您提供更专业的服务！";
					}

				}



			}
			else if (apMa.Success && content.Length <= 4)
			{
				string ap4code = content;
				string apname = "";
				string m = "";//metar
				if (content.Length == 3)
				{
					//消息为3码，取机场4码
					string sqlstr = "select airport_4code from xhfoc.t7001 where airport_3code = '" + content.ToUpper() + "'";
					string ap4co = DbHelperOra.ExecuteScaleString(sqlstr);
					if (!string.IsNullOrEmpty(ap4co))
					{
						ap4code = ap4co;
					}
				}

				sql = "select * from xhfoc.t2012 where airport_4code='" + ap4code.ToUpper() + "' and w_type='M' and rownum=1 order by receive_time desc";
				m = queryInfo(sql);
				sql = "select city_ch_name from xhfoc.t7001 where airport_4code = '" + ap4code.ToUpper() + "'";
				apname = DbHelperOra.ExecuteScaleString(sql);
				if (m == "")
				{
					replayContent = "请输入正确的四字码或三字码";
				}
				else
				{
					replayContent = apname + "机场实况:\n" + m;
					sql = "select * from xhfoc.t2012 where airport_4code='" + ap4code.ToUpper() + "' and w_type='T' and rownum=1 order by receive_time desc";
					replayContent = replayContent + "\n预报:\n" + queryInfo(sql);
				}

			}
			else if (airpMa.Success)
			{
				if (isInternal == "Y")
				{
					switch (content.Substring(content.Length - 1, 1))
					{
						case "1":

							string ap = content.Substring(0, 4);
							sql = "select weather_property from xhfoc.t7001 where airport_4code='" + ap.ToUpper() + "'";
							string qhContent = DbHelperOra.ExecuteScaleString(sql);
							if (string.IsNullOrEmpty(qhContent))
							{
								replayContent = "请输入正确的机场四字码";
							}
							else
							{
								replayContent = qhContent;
							}
							break;
						case "2":
							ap = content.Substring(0, 4);
							sql = "select specific_rule from xhfoc.t7001 where airport_4code='" + ap.ToUpper() + "'";
							string hintContent = DbHelperOra.ExecuteScaleString(sql);
							if (string.IsNullOrEmpty(hintContent))
							{
								replayContent = "请输入正确的机场四字码";
							}
							else
							{
								replayContent = hintContent;
							}
							break;
						case "3":
							RNewsMessage msg_n = new RNewsMessage(inMsg.To, inMsg.From);
							msg_n.AddNews(content.Substring(0, 4) + "雷达", "雷达", @"http://i.weather.com.cn/i/product/pic/l/sevp_aoc_rdcp_sldas_ebref_az9592_l88_pi_20130502011500000.gif", @"http://i.weather.com.cn/i/product/pic/l/sevp_aoc_rdcp_sldas_ebref_az9592_l88_pi_20130502011500000.gif");
							break;
						default:
							replayContent = "请输入zsam1或zsam2";
							break;
					}
				}
				else
				{
					replayContent = "请输入+工号+姓名+岗位加入认证用户！";
				}
			}
			else
			{
				switch (content)
				{
					case "1":
					case "雷雨":
						sql = "select distinct airport_4code from xhfoc.t2012 where sqc in(select max(sqc) from xhfoc.t2012 where (w_type='M' or w_type='S') and (sysdate-receive_time)*24<3  group by airport_4code) and weather_ph like '%TS%' order by airport_4code";
						replayContent = "实况出现雷雨机场：\n" + queryInfo1(sql);
						break;
					case "2":
					case "大雾":
						sql = "select distinct airport_4code from xhfoc.t2012 where sqc in(select max(sqc) from xhfoc.t2012 where (w_type='M' or w_type='S') and (sysdate-receive_time)*24<3 group by airport_4code) and visibility<1000 order by airport_4code ";
						replayContent = "实况能见度低于1000米的机场：\n" + queryInfo1(sql);
						break;
					case "3":
					case "低云":
						sql = "select distinct airport_4code from xhfoc.t2012 where sqc in(select max(sqc) from xhfoc.t2012 where (w_type='M' or w_type='S') and (sysdate-receive_time)*24<3 group by airport_4code) and cloud_height<4 order by airport_4code";
						replayContent = "实况云高低于120米的机场：\n" + queryInfo1(sql);
						break;
					case "4":
					case "大风":
						sql = "select distinct airport_4code from xhfoc.t2012 where sqc in(select max(sqc) from xhfoc.t2012 where (w_type='M' or w_type='S') and (sysdate-receive_time)*24<3 group by airport_4code) and (wind_speed>10 or flurry>13) order by airport_4code";
						replayContent = "实况风速大于10米/秒或阵风大于13米/秒的机场：\n" + queryInfo1(sql);
						break;
					//case "5":
					//case "当日天气":
					//    sql = "select content,modify_time from xhfoc.t2015 where trunc(modify_time)=trunc(sysdate) and to_char(avi_START_TIME,'HH24')='09' and rownum=1 order by modify_time desc";
					//    replayContent = queryWeatherInfo(sql);
					//    break;
					case "5":
					case "最新天气":
						sql = "select content,modify_time from xhfoc.t2015 where trunc(modify_time)=trunc(sysdate) and w_type='W' and to_char(avi_START_TIME,'HH24')<>'00' and to_char(avi_START_TIME,'HH24')<>'09' order by modify_time desc";
						replayContent = "★3小时时段预报：" + queryWeatherInfo(sql) + "\n";
						sql = "select content,modify_time from xhfoc.t2015 where (sysdate-modify_time)*24<3 and w_type='P' order by modify_time desc";
						replayContent = replayContent + "\n★机场预警：" + queryWeatherInfo(sql) + "\n";
						break;
					case "6":
					case "台风信息":
						sql = "select * from xhfoc.t2015 where w_type='T' and (sysdate-modify_time)*24<12 order by modify_time desc";
						if (string.IsNullOrEmpty(queryWeatherInfo(sql)) == false)
						{
							replayContent = "★台风预报：" + queryWeatherInfo(sql) + "\n";
						}
						else
						{
							replayContent = "无最新台风消息！";
						}
						break;
					case "？":
					case "?":
						replayContent = "查询方法：\n★发送航班号：航班时刻及相关机场天气实况；\n"
					+ "★发送机场三字码、四字码或机场名称（例：xmn、zsam、南昌、虹桥）：机场最新实况及预报报文；\n"
					+ "★发送“雷雨”或“1”：实况有雷雨的机场；\n★“大雾”或“2”：实况出现大雾的机场；\n★“低云”或“3”：实况出现低云的机场；\n★“大风”或“4”：实况出现大风的机场；\n"
					+ "★发送“最新天气”或“5”：查询最新天气形势和机场预警。\n★发送“台风信息”或“6”：查询台风信息。★发送“+工号+姓名+岗位”可加入认证用户。";
						break;
					case "+":
						sql = "select * from WeiXinUsr where username='" + inMsg.From + "'";
						string usr = DbHelperSql.ExecuteScaleString(sql);
						if (string.IsNullOrEmpty(usr))
						{
							sql = "insert into WeiXinUsr(username) values('" + inMsg.From + "')";
							int exrows = DbHelperSql.ExecuteSql(sql);
							replayContent = "感谢您的关注，已将您添加到常用用户组。";
						}
						else
						{
							replayContent = "您已加入常用用户组，请勿重复添加！";
						}
						break;
					case "全国":
					case "华北":
					case "东北":
					case "华中":
					case "华东":
					case "华南":
					case "西南":
					case "西北":
						sql = "select top 1 link from RadarLink where site='" + content + "' order by sqc asc";
						string url = "http://image.weather.gov.cn" + DbHelperSql.ExecuteScaleString(sql);
						url = url.Replace("/medium", "");
						RNewsMessage msg_n = new RNewsMessage(inMsg.To, inMsg.From);
						msg_n.AddNews(content + "雷达", content + "雷达", url, url);
						//HttpResponseWrapper respimg = (HttpResponseWrapper)(args);
						//respimg.Write(msg_n.ToString());
						return msg_n;
					//break;
					default:
						replayContent = queryCity(content);
						if (replayContent.Trim() == "")
							replayContent = "查询方法：\n★发送航班号：航班时刻及相关机场天气实况；\n"
						+ "★发送机场三字码、四字码或机场名称（例：xmn、zsam、南昌、虹桥）：机场最新实况及预报报文；\n"
						+ "★发送“雷雨”或“1”：实况有雷雨的机场；\n★“大雾”或“2”：实况出现大雾的机场；\n★“低云”或“3”：实况出现低云的机场；\n★“大风”或“4”：实况出现大风的机场；\n"
						+ "★发送“最新天气”或“5”：查询最新天气形势和机场预警；\n★发送“台风信息”或“6”：查询台风信息。\n★发送“+工号+姓名+岗位”可加入认证用户。";
						break;
				}

			}

			RTextMessage msg_r = new RTextMessage(inMsg.To, inMsg.From, replayContent);

			return msg_r;
		}

		private static string queryInfo(string sqlstr)
		{
			string metar = "";

			DataSet ds = DbHelperOra.Execute(sqlstr);
			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				metar = ds.Tables[0].Rows[0]["CONTENT"].ToString();

				if (string.IsNullOrEmpty(metar) == true)
				{
					metar = "请确认输入正确的机场四字码！";
				}
			}
			return metar;
		}

		//private static string queryCity(string city)
		//{

		//    MetarDecode msg = new MetarDecode("", "", "");
		//    string metar = "";
		//    string airp = "";
		//    string sqlstr = "select airport_4code from xhfoc.t7001 where city_ch_name='" + city + "'";
		//    DataSet ds = DbHelperOra.Execute(sqlstr);
		//    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
		//    {
		//        airp = ds.Tables[0].Rows[0]["airport_4code"].ToString();

		//        if (string.IsNullOrEmpty(airp) == false)
		//        {
		//            sqlstr = "select * from xhfoc.t2012 where airport_4code='" + airp.ToUpper() + "' and w_type='M' and rownum=1 order by receive_time desc";
		//            DataSet ds1 = DbHelperOra.Execute(sqlstr);
		//            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
		//            {
		//                msg = new MetarDecode(ds1.Tables[0].Rows[0]["WIND_DIRECTION"].ToString(), ds1.Tables[0].Rows[0]["WEATHER_PH"].ToString(), ds1.Tables[0].Rows[0]["CLOUD_QUANTITY"].ToString());
		//                msg.avi_start_time = Convert.ToDateTime(ds1.Tables[0].Rows[0]["AVI_START_TIME"].ToString());
		//                msg.wind_direction = ds1.Tables[0].Rows[0]["WIND_DIRECTION"].ToString();
		//                msg.wind_speed = Convert.ToInt32(ds1.Tables[0].Rows[0]["WIND_SPEED"].ToString());
		//                if (string.IsNullOrEmpty(ds1.Tables[0].Rows[0]["VISIBILITY"].ToString()) == true && ds1.Tables[0].Rows[0]["CAVOK"].ToString() == "Y")
		//                {
		//                    msg.visibility = "大于等于10000";
		//                }
		//                else
		//                {
		//                    msg.visibility = ds1.Tables[0].Rows[0]["VISIBILITY"].ToString();
		//                }
		//                if (string.IsNullOrEmpty(ds1.Tables[0].Rows[0]["CLOUD_HEIGHT"].ToString()) == true && ds1.Tables[0].Rows[0]["CAVOK"].ToString() == "Y")
		//                {
		//                    msg.cloud_height = "大于1500米或最高的最低扇区安全高度";
		//                }
		//                else
		//                {
		//                    if (string.IsNullOrEmpty(ds1.Tables[0].Rows[0]["CLOUD_HEIGHT"].ToString()))
		//                    {
		//                        if (ds1.Tables[0].Rows[0]["CAVOK"].ToString() == "Y" || ds1.Tables[0].Rows[0]["WEATHER_PH"].ToString().IndexOf("SKC") >= 0 || ds1.Tables[0].Rows[0]["WEATHER_PH"].ToString().IndexOf("NSC") >= 0)
		//                        {
		//                            msg.cloud_height = "大于1500米或最高的最低扇区安全高度";
		//                        }
		//                        else
		//                        {
		//                            msg.cloud_height = "未知";
		//                        }
		//                    }
		//                    else
		//                    {
		//                        msg.cloud_height = (Convert.ToInt32(ds1.Tables[0].Rows[0]["CLOUD_HEIGHT"].ToString()) * 30).ToString() + "米";
		//                    }
		//                }
		//                msg.temperature = Convert.ToInt32(ds1.Tables[0].Rows[0]["TEMPERATURE"].ToString());
		//                msg.dew_point = Convert.ToInt32(ds1.Tables[0].Rows[0]["DEW_POINT"].ToString());
		//                msg.air_pressure = Convert.ToInt32(ds1.Tables[0].Rows[0]["AIR_PRESSURE"].ToString());
		//                metar = msg.avi_start_time.ToString("yyyy-MM-dd HH:mm")
		//                            + "实况：\n风向：" + msg.wind_direction + "° 风速：" + msg.wind_speed.ToString() + "米/秒 能见度："
		//                            + msg.visibility.ToString() + "米 天气：" + msg.weather_ph + " 云量：" + msg.cloud_quantity
		//                            + "  云高：" + msg.cloud_height + " 温度：" + msg.temperature.ToString() + "°C 露点：" + msg.dew_point.ToString()
		//                            + "℃ 修正海压：" + msg.air_pressure.ToString() + "百帕";
		//            }
		//        }
		//    }
		//    else
		//    {
		//        metar = "请输入正确的机场";
		//    }
		//    return metar;
		//}

		private static string queryCity(string city)
		{

			string metar = "";
			string sqlstr = "select t1.airport_4code,t2.content ct from xhfoc.t7001 t1,xhfoc.t2012 t2 where t1.city_ch_name='" + city
				+ "'  and t1.airport_4code=t2.airport_4code and (t2.w_type='M' or t2.w_type='S') order by t2.receive_time desc";
			DataSet ds = DbHelperOra.Execute(sqlstr);
			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				metar = city + "机场实况：\n" + ds.Tables[0].Rows[0]["ct"].ToString() + "\n";
				sqlstr = "select t1.airport_4code,t2.content ct from xhfoc.t7001 t1,xhfoc.t2012 t2 where t1.city_ch_name='" + city
	+ "'  and t1.airport_4code=t2.airport_4code and t2.w_type='T' order by t2.avi_start_time desc";
				ds = DbHelperOra.Execute(sqlstr);
				metar += "预报：\n" + ds.Tables[0].Rows[0]["ct"].ToString();
			}
			else
			{
				metar = "请输入正确的机场或指令，如需帮助请输入？";
			}
			return metar;
		}

		private static string queryWeatherInfo(string sqlstr)
		{
			string metar = "";

			DataSet ds = DbHelperOra.Execute(sqlstr);
			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				metar = ds.Tables[0].Rows[0]["CONTENT"].ToString();
				metar += "  发布时间：" + ds.Tables[0].Rows[0]["MODIFY_TIME"].ToString();
				if (string.IsNullOrEmpty(metar) == true)
				{
					metar = "";
				}
			}
			return metar;
		}

		private static string queryInfo1(string sqlstr)
		{
			string metar = "";
			DataSet ds = DbHelperOra.Execute(sqlstr);
			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					metar += dr["airport_4code"].ToString() + ",";
				}
			}
			else
			{
				metar = "未查到机场";
			}
			return metar;
		}
	}
}