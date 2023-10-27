using System;
using System.Collections;
using UnityEngine.Networking;

namespace NetworkProcessor {
    public class BasicNetworkHandler {
        protected NetworkConfig networkConfig = new();

        /// <summary>
        /// A Func Which Start A Get Request, Which Suit For Multi-Scenes
        /// </summary>
        /// <param name="urlSuffix">Target URL Path Follows Server Address In "NetworkConfig" Class</param>
        /// <param name="progressCallback">Return the Request Progress</param>
        /// <param name="suffixCallback">Execute with 'True' if succeed either 'False'</param>
        /// <returns>An IEnumerator</returns>
        public virtual IEnumerator UniversalGet(
            string urlSuffix,
            Action<object> progressCallback = null,
            Action<object, bool> suffixCallback = null
        ) {
            var getRequestHandler = UnityWebRequest.Get(networkConfig.ServerAddress + urlSuffix);

            var request = getRequestHandler.SendWebRequest();

            // Waiting For Requests
            if (progressCallback is not null)
                while (request.isDone == false) {
                    progressCallback.Invoke(request.progress);
                    yield return null;
                }
            else yield return request;

            // Requests Completed
            if (getRequestHandler.result == UnityWebRequest.Result.Success) {
                // If Succeed, Reflect on Progress
                progressCallback?.Invoke(request.progress);

                suffixCallback?.Invoke(getRequestHandler.downloadHandler.text, true);
            }
            else {
                suffixCallback?.Invoke(getRequestHandler.error, false);
            }
        }
    }
}