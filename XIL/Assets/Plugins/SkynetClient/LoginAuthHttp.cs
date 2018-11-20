
using UnityEngine;
using  CI.HttpClient;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace SkynetClient
{
    public class LoginwReqPack
    {
        public string module = "login_auth";
        public string method = "login";
        public string server_ca = "hDJ^54D@!&DHkkdh095hj";
        public string parms = "'";
    }

    public delegate void LoginAuthRespCallBack(AuthPackageResp resp);

    public class LoginAuthHttp
    {
        private static HttpClient client =  new HttpClient();
        private static string url = "http://192.168.1.25:15110/api";
        private static LoginAuthRespCallBack respcb;
        public static void DoLoginReqAction(AuthPackage cmd, LoginAuthRespCallBack cb)
        {
            respcb = cb;
            LoginwReqPack req = new LoginwReqPack();
            req.parms = JsonConvert.SerializeObject(cmd);
            //HttpResponseMessage response = null;
            string jsonstr = JsonConvert.SerializeObject(req);

            IHttpContent content = new StringContent(jsonstr, System.Text.Encoding.UTF8, "application/json");
            client.Post(new Uri(url), content, HttpCompletionOption.AllResponseContent, r =>
            {
                if (r.IsSuccessStatusCode)
                {
                    string resp_str = r.ReadAsString();
                    try
                    {
                        AuthPackageResp resp = JsonConvert.DeserializeObject<AuthPackageResp>(resp_str);
                        respcb(resp);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError("Json Deserialize err:" + e.Message.ToString());
                    }
                }
            });
        }
    }

}