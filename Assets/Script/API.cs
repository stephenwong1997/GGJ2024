using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

public static class API
{
    public static async UniTask<PlatformSettings> GetJoystickSettingsAsync()
    {
        string url = "https://api.getjoystick.com/api/v1/config/ggj2024_production/dynamic";
        string requestJSON = "{}";

        WebServiceResponse response = await CallWebServiceAsync(url, requestJSON);
        if (response.Status != ReturnStatus.OK)
        {
            Debug.LogError($"GetJoystickSettingsAsync Response Status: {response.Status}, JSON: {response.JSON}.");
            return null;
        }

        ConfigItem configItem = JsonConvert.DeserializeObject<ConfigItem>(response.JSON);
        return configItem.data.platformSettings;
    }

    private static async UniTask<WebServiceResponse> CallWebServiceAsync(string url, string requestJSON)
    {
        byte[] requestBytes = new System.Text.UTF8Encoding().GetBytes(requestJSON);

        UnityWebRequest webRequest = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(requestBytes),
            downloadHandler = new DownloadHandlerBuffer()
        };

        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("X-Api-Key", "VRRDZLVDIBCzUx0weurD3t69ihrKMDeE");

        try
        {
            await webRequest.SendWebRequest();
        }
        catch (Exception e)
        {
            Debug.LogException(e);

            webRequest.Dispose();
            return new WebServiceResponse(ReturnStatus.ERROR, e.Message);
        }

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Web service call failed: " + webRequest.error);
            if (webRequest.responseCode == 401)
            {
                webRequest.Dispose();
                return new WebServiceResponse(ReturnStatus.UNAUTHORIZED, "");
            }

            webRequest.Dispose();
            return new WebServiceResponse(ReturnStatus.ERROR, "");
        }

        string responseJSON = webRequest.downloadHandler.text;
        Debug.Log("Web service response: " + responseJSON);

        webRequest.Dispose();
        return new WebServiceResponse(ReturnStatus.OK, responseJSON);
    }

    // HELPERS
    private struct WebServiceResponse
    {
        public ReturnStatus Status { get; private set; }
        public string JSON { get; private set; }

        public WebServiceResponse(ReturnStatus returnStatus, string json)
        {
            Status = returnStatus;
            JSON = json;
        }
    }


    [System.Serializable]
    public class ConfigItem
    {
        [JsonProperty("data")]
        public ConfigData data;
        public Dictionary<string, object> meta;
    }

    [System.Serializable]
    public class ConfigData
    {
        [JsonProperty("platform-settings")]
        public PlatformSettings platformSettings;
    }

    /*
    "data": {
        "platform-settings": {
            "min-spawn-frequency": 1.4,
            "max-spawn-frequency": 2.5,
            "total-game-time": 20,
            "spawn-finish-line-delay": 2,
            "platform-speed": 5,
            "item-spawn-probability": 1
        }
    },
    */

    [System.Serializable]
    public class PlatformSettings
    {
        [JsonProperty("min-spawn-frequency")]
        public float minSpawnFrequency;

        [JsonProperty("max-spawn-frequency")]
        public float maxSpawnFreqeuncy;

        [JsonProperty("total-game-time")]
        public float totalGameTime;

        [JsonProperty("spawn-finish-line-delay")]
        public float spawnFinishLineDelay;

        [JsonProperty("platform-speed")]
        public float platformSpeed;

        [JsonProperty("item-spawn-probability")]
        public float itemSpawnProbability;
    }
}

[System.Serializable]
public class ConfigItem
{
    [JsonProperty("data")]
    public ConfigData data;
    public Dictionary<string, object> meta;
}

[System.Serializable]
public class ConfigData
{
    [JsonProperty("platform-settings")]
    public PlatformSettings platformSettings;
}

/*
"data": {
    "platform-settings": {
        "min-spawn-frequency": 1.4,
        "max-spawn-frequency": 2.5,
        "total-game-time": 20,
        "spawn-finish-line-delay": 2,
        "platform-speed": 5,
        "item-spawn-probability": 1
    }
},
*/

[System.Serializable]
public class PlatformSettings
{
    [JsonProperty("min-spawn-frequency")]
    public float minSpawnFrequency;

    [JsonProperty("max-spawn-frequency")]
    public float maxSpawnFreqeuncy;

    [JsonProperty("total-game-time")]
    public float totalGameTime;

    [JsonProperty("spawn-finish-line-delay")]
    public float spawnFinishLineDelay;

    [JsonProperty("platform-speed")]
    public float platformSpeed;

    [JsonProperty("item-spawn-probability")]
    public float itemSpawnProbability;
}

public enum ReturnStatus
{
    OK,
    ERROR,
    FAIL,
    UNAUTHORIZED
}