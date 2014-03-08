using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXin.Models.API
{
    public class APIResult
    {
        public Error error { get; set; }
        public AccessToken accessToken { get; set; }
        public MediaUploadStatus mediaUploadStatus { get; set; }

        public UserInfo userInfo { get; set; }

        public AddGroupResult addGroupResult { get; set; }
        public Groups groups { get; set; }
        public UserGroup userGroup { get; set; }
        public string returnString { get; set; }

        //public object

        public APIResult()
        {
            this.error = Error.GetOK();
        }
    }
}
