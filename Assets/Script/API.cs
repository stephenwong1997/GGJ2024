using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

public static class API
{
    public static async UniTask GetJoystickSettingsAsync()
    {
        string url = "https://api.getjoystick.com/api/v1/config/ggj2024_production/dynamic";
        string requestJSON = "{}";

        WebServiceResponse response = await CallWebServiceAsync(url, requestJSON);
        if (response.Status != ReturnStatus.OK)
        {
            Debug.LogError($"GetJoystickSettingsAsync Response Status: {response.Status}, JSON: {response.JSON}.");
            return;
        }

        // TODO
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
}

public enum ReturnStatus
{
    OK,
    ERROR,
    FAIL,
    UNAUTHORIZED
}