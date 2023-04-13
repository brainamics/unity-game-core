using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Brainamics.Core
{
    public class WorldTimeApiTimeProvider : ITimeProvider
    {
        private const string ApiUrl = "http://worldtimeapi.org/api/timezone/etc/utc";

        public async Task<DateTime> GetTimeAsync()
        {
            using var request = UnityWebRequest.Get(ApiUrl);
            await request.SendWebRequest().AsTask();
            if (request.result != UnityWebRequest.Result.Success)
                throw new Exception(request.error);
            var response = JsonUtility.FromJson<ResponseDto>(request.downloadHandler.text);
            return DateTime.Parse(response.utc_datetime);
        }

        [Serializable]
        private sealed class ResponseDto
        {
#pragma warning disable CS0649
            public string datetime;
            public string utc_datetime;
#pragma warning restore CS0649
        }
    }
}