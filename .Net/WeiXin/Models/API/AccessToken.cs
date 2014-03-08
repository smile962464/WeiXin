using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace WeiXin.Models.API
{
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string Access_Token { get; set; }
        [JsonProperty("expires_in")]
        public string Expires_In { get; set; }
    }
}
