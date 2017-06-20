using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace LuckyNumber.SMSAPI
{
    public class SpeedSMSAPI
    {
        public const int TYPE_QC = 1;
        public const int TYPE_CSKH = 2;
        public const int TYPE_BRANDNAME = 3;
        const String rootURL = "http://api.speedsms.vn/index.php";
        private String accessToken = "nPF0bn6Kvg83JUiAU3LZIr_Jovof7hpE";

        public SpeedSMSAPI()
        {

        }

        public SpeedSMSAPI(String token)
        {
            this.accessToken = token;
        }

        public String getUserInfo()
        {
            String url = rootURL + "/user/info";
            NetworkCredential myCreds = new NetworkCredential(accessToken, ":x");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            return reader.ReadToEnd();
        }

        public String sendSMS(String phone, String content, int type, String brandname)
        {
            String url = rootURL + "/sms/send";
            //if (phone.Length <= 0 || phone.Length < 10 || phone.Length > 11)
            //    return "";
            if (content.Equals(""))
                return "";
            if (type < TYPE_QC || type > TYPE_BRANDNAME)
                return "";
            if (type == TYPE_BRANDNAME && brandname.Equals(""))
                return "";
            if (!brandname.Equals("") && brandname.Length > 11)
                return "";

            NetworkCredential myCreds = new NetworkCredential(accessToken, ":x");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            String json = "{\"to\":[\"" + phone + "\"], \"content\": \"" + content + "\", \"type\":" + type + ", \"brandname\": \"" + brandname + "\"}";
            return client.UploadString(url, json);
        }
    }
}