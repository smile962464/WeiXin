using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace WeiXin.Models.API
{
    public class SubscriberList
    {
        /**
        {
            "total":2,
            "count":2,
            "data":
            {
                "openid":["","OPENID1","OPENID2"]
            },
            "next_openid":"NEXT_OPENID"
        }
         */
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }
        [JsonProperty("next_openid")]
        public string NextOpenid { get; set; }
    }
}
