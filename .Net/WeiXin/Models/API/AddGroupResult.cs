using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace WeiXin.Models.API
{
    public class AddGroupResult
    {
        /**
        {
            "group": {
                "id": 107, 
                "name": "test"
            }
        }
        */
        [JsonProperty("group")]
        public dynamic Group { get; set; }
    }
}
