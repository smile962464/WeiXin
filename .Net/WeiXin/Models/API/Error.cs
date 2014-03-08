using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace WeiXin.Models.API
{
    public class Error
    {
        [JsonProperty("errcode")]
        public string ErrCode { get; set; }
        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; }

        public static Error GetOK()
        {
            Error err = new Error();
            err.ErrCode = "0";
            err.ErrMsg = "ok";
            return err;
        }
    }
}
