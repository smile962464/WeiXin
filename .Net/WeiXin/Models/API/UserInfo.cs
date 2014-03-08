using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace WeiXin.Models.API
{
    public class UserInfo
    {
        //{
        //    "subscribe": 1, 
        //    "openid": "o6_bmjrPTlm6_2sgVt7hMZOPfL2M", 
        //    "nickname": "Band", 
        //    "sex": 1, 
        //    "language": "zh_CN", 
        //    "city": "广州", 
        //    "province": "广东", 
        //    "country": "中国", 
        //    "headimgurl":    "http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0", 
        //   "subscribe_time": 1382694957
        //}
        [JsonProperty("subscribe")]
        public int Subscribe { get; set; }
        [JsonProperty("openid")]
        public string OpenId { get; set; }
        [JsonProperty("nickname")]
        public string NickName { get; set; }
        [JsonProperty("sex")]
        public int Sex { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("province")]
        public string Province { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("headimgurl")]
        public string HeadImgUrl { get; set; }
        [JsonProperty("subscribe_time")]
        public long SubscribeTime { get; set; }
    }
}
