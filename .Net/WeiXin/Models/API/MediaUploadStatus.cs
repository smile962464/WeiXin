using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace WeiXin.Models.API
{
    public class MediaUploadStatus
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("media_id")]
        public string MediaId { get; set; }
        [JsonProperty("created_at")]
        public string CreateAt { get; set; }
    }
}
