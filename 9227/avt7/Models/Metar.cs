using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace avt7.Models
{
    public class MetarDecode
    {
        public DateTime avi_start_time;
        public DateTime avi_end_time;
        public string wind_direction;
        public int wind_speed;
        public string visibility;
        public int rvr;
        public string weather_ph="";
        public string cloud_quantity="";
        public string cloud_height;
        public int temperature;
        public int dew_point;
        public int air_pressure;
        public char cavok;

        public MetarDecode(string wd, string wx, string cl)
        {
            if (wd == "VRB")
            {
                wind_direction = "不定";
            }
            else
            {
                wind_direction = wd.Replace('0', ' ') + "°";
            }
            switch (wx.Trim())
            {
                case "MIFG":
                    weather_ph = "浅雾";
                    break;
                case "FG":
                    weather_ph = "大雾";
                    break;
                case "BR":
                    weather_ph = "轻雾";
                    break;
                case "TS":
                    weather_ph = "干雷";
                    break;
                case "-TSRA":
                    weather_ph = "小雷雨";
                    break;
                case "TSRA":
                    weather_ph = "中雷雨";
                    break;
                case "+TSRA":
                    weather_ph = "强雷雨";
                    break;
                case "-DZ":
                    weather_ph = "小毛毛雨";
                    break;
                case "DZ":
                    weather_ph = "毛毛雨";
                    break;
                case "+DZ":
                    weather_ph = "大毛毛雨";
                    break;
                case "-RA":
                    weather_ph = "小雨";
                    break;
                case "RA":
                    weather_ph = "中雨";
                    break;
                case "+RA":
                    weather_ph = "大雨";
                    break;
                case "-SN":
                    weather_ph = "小雪";
                    break;
                case "SN":
                    weather_ph = "中雪";
                    break;
                case "+SN":
                    weather_ph = "大雪";
                    break;
                case "FU":
                    weather_ph = "烟";
                    break;
                case "DU":
                    weather_ph = "浮尘";
                    break;
            }
            switch (cl)
            {
                case "FEW":
                    cloud_quantity = "少云";
                    break;
                case "SCT":
                    cloud_quantity = "疏云";
                    break;
                case "BKN":
                    cloud_quantity = "多云";
                    break;
                case "OVC":
                    cloud_quantity = "阴天";
                    break;
                case "NSC":
                    cloud_quantity = "无重要云";
                    break;
                case "SKC":
                    cloud_quantity = "无云";
                    break;
            }
        }
    }

}
