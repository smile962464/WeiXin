using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using System.Collections.Specialized;
using System.Text.RegularExpressions;

using WeiXin.Models;
using WeiXin.Models.API;

namespace WeiXin
{
    public static class Utities
    {

        //// AccessToken //////////
        public static string AccessTokenString(string appid = "wx8980382cb43a6897", string appsecret = "1137740a67c5152a9cd7af37abb4cd28")
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + appsecret;
            using (var wc = new System.Net.WebClient())
            {
                var data = wc.DownloadData(url);
                string accessToken = Utities.GetStringFromBytes(data);
                return accessToken;
            }
        }
        public static APIResult AccessToken(string appid = "wx8980382cb43a6897", string appsecret = "1137740a67c5152a9cd7af37abb4cd28")
        {
            APIResult result = new APIResult();
            var act = Utities.AccessTokenString(appid, appsecret);
            result.returnString = act;
            if (act.IndexOf("errmsg") != -1)
            {
                result.error = Utities.DeserializeObject<Error>(act);
            }
            else
            {
                result.accessToken = Utities.DeserializeObject<AccessToken>(act);
            }
            return result;
        }

        //// ServiceMessage客服消息 //////////
        public static APIResult SendServiceMessage(string accessToken, RMessage msg)
        {
            using (var wc = new System.Net.WebClient())
            {
                APIResult result = new APIResult();
                var data = Utities.GetBytesFromString(msg.ToJsonString());
                string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + accessToken;
                var res = Utities.GetStringFromBytes(wc.UploadData(url, data));
                result.returnString = res;
                result.error = Utities.DeserializeObject<Error>(res);
                return result;
            }
        }

        ////  Uplaod Media  ////////
        public static APIResult UploadMedia(string accessToken, string type, string file)
        {
            using (var wc = new System.Net.WebClient())
            {
                APIResult result = new APIResult();
                string url = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + accessToken + "&type=" + type;

                var res = Utities.GetStringFromBytes(wc.UploadFile(url, "POST", file));
                result.returnString = res;
                if (res.IndexOf("errmsg") != -1)
                {
                    result.error = Utities.DeserializeObject<Error>(res);
                }
                else
                {
                    result.mediaUploadStatus = Utities.DeserializeObject<MediaUploadStatus>(res);
                }
                return result;
            }
        }

        ////  Downlaod Media  ////////
        public static APIResult DownloadMedia(string accessToken, string mediaId, string path, string name = null)
        {
            APIResult result = new APIResult();
            using (var wc = new System.Net.WebClient())
            {
                string url = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + accessToken + "&media_id=" + mediaId;
                //wc.DownloadFile(url, path);
                var _res = wc.DownloadData(url);
                var res = Utities.GetStringFromBytes(_res);

                var content_type = wc.ResponseHeaders["Content-Type"];
                //错误
                if (content_type.ToUpper().IndexOf("TEXT") != -1)
                {
                    result.error = Utities.DeserializeObject<Error>(res);
                    result.returnString = res;
                }
                else
                {
                    var fileType = content_type.Substring(0, content_type.IndexOf("/"));
                    var fileExt = content_type.Substring(content_type.IndexOf("/") + 1);
                    var fileName = string.IsNullOrEmpty(name) ?
                        fileType + "_" + Utities.GetTimeTicks(DateTime.Now).ToString() + "." + fileExt
                        : name + "." + fileExt;
                    var _path = path + fileName;
                    if (File.Exists(_path))
                    {
                        File.Delete(_path);
                    }
                    FileStream fs = new FileStream(_path, FileMode.CreateNew, FileAccess.Write);
                    fs.Write(_res, 0, _res.Length);
                }
                return result;
            }
        }

        //// Menu //////////
        public static APIResult AddMenuFromFile(string accessToken, string path)
        {
            System.IO.FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            var menu = sr.ReadToEnd();
            APIResult status = AddMenu(accessToken, menu);
            sr.Close();
            fs.Close();
            return status;
        }

        public static APIResult AddMenu(string accessToken, string menu)
        {
            using (var wc = new System.Net.WebClient())
            {
                APIResult result = new APIResult();
                var data = Utities.GetBytesFromString(menu);
                var url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + accessToken;
                var res = Utities.GetStringFromBytes(wc.UploadData(url, data));
                result.error = Utities.DeserializeObject<Error>(res);
                result.returnString = res;
                return result;
            }
        }

        public static APIResult GetMenu(string accessToken)
        {
            using (var wc = new System.Net.WebClient())
            {
                APIResult result = new APIResult();
                var url = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + accessToken;
                var res = Utities.GetStringFromBytes(wc.DownloadData(url));
                result.returnString = res;
                if (res.IndexOf("errmsg") != -1)
                {
                    result.error = Utities.DeserializeObject<Error>(res);
                }
                return result;
            }
        }

        public static APIResult DeleteMenu(string accessToken)
        {
            using (var wc = new System.Net.WebClient())
            {
                APIResult result = new APIResult();
                var url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + accessToken;
                var res = Utities.GetStringFromBytes(wc.DownloadData(url));
                result.returnString = res;
                result.error = Utities.DeserializeObject<Error>(res);
                return result;
            }
        }


        /////   Get Subscriber List /////
        public static List<string> GetSubscriberList(string accessToken, string next = null)
        {
            List<string> slist = new List<string>();
            using (var wc = new System.Net.WebClient())
            {
                var count = 0;
                var total = 1;
                while (count < total)
                {
                    var url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + accessToken;
                    if (!string.IsNullOrEmpty(next))
                    {
                        url += ("&next_openid=" + next);
                    }
                    var res = Utities.GetStringFromBytes(wc.DownloadData(url));
                    SubscriberList l = Utities.DeserializeObject<SubscriberList>(res);

                    dynamic _list = l.Data;
                    foreach (string openid in _list.openid)
                    {
                        slist.Add(openid);
                    }
                    total = l.Total;
                    count += l.Count;
                }
            }
            return slist;
        }

        /////   GetUers Information /////
        public static APIResult GetUserInfo(string accessToken, string openId, string lang = "zh_CN")
        {
            using (var wc = new System.Net.WebClient())
            {
                APIResult result = new APIResult();
                var url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + accessToken + "&openid=" + openId + "&lang=" + lang;
                var res = Utities.GetStringFromBytes(wc.DownloadData(url));
                result.returnString = res;
                if (res.IndexOf("errmsg") != -1)
                {
                    result.error = Utities.DeserializeObject<Error>(res);
                }
                else
                {
                    result.userInfo = Utities.DeserializeObject<UserInfo>(res);
                }
                return result;
            }
        }

        ///// 分组操作 ///////
        ///// 添加分组 ///////
        public static APIResult AddGroup(string accessToken, string groupName)
        {
            using (var wc = new System.Net.WebClient())
            {
                APIResult result = new APIResult();
                var url = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token=" + accessToken;
                var _group = new { group = new { name = groupName } };
                var group = Utities.GetBytesFromString(Utities.JsonSerialize(_group));
                var res = Utities.GetStringFromBytes(wc.UploadData(url, group));
                result.returnString = res;

                if (res.IndexOf("errmsg") != -1)
                {
                    result.error = Utities.DeserializeObject<Error>(res);
                }
                else
                {
                    result.addGroupResult = Utities.DeserializeObject<AddGroupResult>(res);
                }
                return result;
            }
        }

        ///// 获取分组 ///////
        public static APIResult GetAllGroups(string accessToken)
        {
            using (var wc = new System.Net.WebClient())
            {
                APIResult result = new APIResult();
                var url = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token=" + accessToken;
                var res = Utities.GetStringFromBytes(wc.DownloadData(url));
                result.returnString = res;

                if (res.IndexOf("errmsg") != -1)
                {
                    result.error = Utities.DeserializeObject<Error>(res);
                }
                else
                {
                    result.groups = Utities.DeserializeObject<Groups>(res);
                }
                return result;
            }
        }

        ///// 查询用户所在分组   ///////
        public static APIResult GetUserGroup(string accessToken, string openId)
        {
            using (var wc = new System.Net.WebClient())
            {
                APIResult result = new APIResult();
                var url = "https://api.weixin.qq.com/cgi-bin/groups/getid?access_token=" + accessToken;
                var _group = new { openid = openId };
                var group = Utities.GetBytesFromString(Utities.JsonSerialize(_group));
                var res = Utities.GetStringFromBytes(wc.UploadData(url, group));
                result.returnString = res;

                if (res.IndexOf("errmsg") != -1)
                {
                    result.error = Utities.DeserializeObject<Error>(res);
                }
                else
                {
                    result.userGroup = Utities.DeserializeObject<UserGroup>(res);
                }
                return result;
            }
        }

        ///修改分组名
        public static APIResult ModeifyGroup(string accessToken, string groupId, string newName)
        {
            using (var wc = new System.Net.WebClient())
            {
                APIResult result = new APIResult();
                var url = "https://api.weixin.qq.com/cgi-bin/groups/update?access_token=" + accessToken;
                var _group = new { group = new { id = groupId, name = newName } };
                var group = Utities.GetBytesFromString(Utities.JsonSerialize(_group));
                var res = Utities.GetStringFromBytes(wc.UploadData(url, group));
                result.returnString = res;
                result.error = Utities.DeserializeObject<Error>(res);
                return result;
            }
        }

        ///移动用户分组
        public static APIResult MoveUserToGroup(string accessToken, string openId, string toGroupId)
        {
            using (var wc = new System.Net.WebClient())
            {
                APIResult result = new APIResult();
                var url = "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token=" + accessToken;
                var _group = new { openid = openId, to_groupid = toGroupId };
                var group = Utities.GetBytesFromString(Utities.JsonSerialize(_group));
                var res = Utities.GetStringFromBytes(wc.UploadData(url, group));
                result.returnString = res;
                result.error = Utities.DeserializeObject<Error>(res);
                return result;
            }
        }

        //// Utils //////////
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

        public static string GetStringFromBytes(byte[] data)
        {
            return System.Text.Encoding.UTF8.GetString(data);
        }
        public static byte[] GetBytesFromString(string data)
        {
            byte[] _data = System.Text.Encoding.UTF8.GetBytes(data);
            return _data;
        }

        public static string JsonSerialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static T DeserializeObject<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);

        }
        public static T DeserializeAnonymousType<T>(string json, T define)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, define);
        }
    }
}
