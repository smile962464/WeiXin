using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace WeiXin.Models.API
{
    public class Group
    {
        /**
        {
            "id": 0, 
            "name": "未分组", 
            "count": 72596
        }        
         */
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("count")]
        public string Count { get; set; }
    }
    public class Groups
    {
        [JsonProperty("groups")]
        public List<Group> Group { get; set; }
    }

    public class UserGroup
    {
        /**
        {
            "groupid": 102
        }
         */
        [JsonProperty("groupid")]
        public string GroupId { get; set; }
    }
}
