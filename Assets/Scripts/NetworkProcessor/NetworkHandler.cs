using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NetworkProcessor {
    public class NetworkHandler : BasicNetworkHandler {

        public const string LoginRoute = "/user_login";
        public const string RegisterRoute = "/user_register";
        public const string DataVerInterface = "/data_version";
        
        
        public override IEnumerator UniversalGet(string urlSuffix, Action<object> progressCallback = null, Action<object, bool> suffixCallback = null) {
            return base.UniversalGet(urlSuffix, progressCallback, suffixCallback);
        }

        public string SerializingGetParams(string suffix, Dictionary<string, string> dic) {
            StringBuilder result = new (suffix);
            result.Append("?");
            foreach (var key in dic.Keys) {
                result.Append(key);
                result.Append("=");
                result.Append(dic[key]);
                result.Append("&");
            }
            
            return result.ToString();
        }
    }
}