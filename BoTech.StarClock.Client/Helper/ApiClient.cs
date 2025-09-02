using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using BoTech.StarClock.Api.SharedModels;
using Newtonsoft.Json;

namespace BoTech.StarClock.Client.Helper;

public class ApiClient
{
    private HttpRequestHelper _httpRequestHelper;

    private ApiClient(HttpRequestHelper httpRequestHelper)
    {
        _httpRequestHelper = httpRequestHelper;
    }
    /// <summary>
    /// Creates an ApiClient, which can be used to access a specific api, which is hosted on a specific IpAddress
    /// </summary>
    /// <param name="baseUrl">the url of the api</param>
    /// <returns>The instance of the client when a connection can be established and the error or success message.</returns>
    public static (ApiClient? client, HttpResponseMessage? message) CreateApiClientFor(string baseUrl)
    {
        HttpRequestHelper requestHelper = new HttpRequestHelper(baseUrl);
        (bool connectionSuccess, HttpResponseMessage message) result = TryToConnectToNewDevice(requestHelper);
        if(result.connectionSuccess) return (new ApiClient(requestHelper), result.message);
        return (null, result.message);
    }
    /// <summary>
    /// Check if the given Device is still reachable in this network.
    /// </summary>
    /// <param name="requestHelper"></param>
    /// <returns></returns>
    private static (bool connectionSuccess, HttpResponseMessage message) TryToConnectToNewDevice(
        HttpRequestHelper requestHelper)
    {
        (HttpResponseMessage? httpResponse, string? data) response = requestHelper.HttpGetString("/Status/TestConnection");
        if(response.httpResponse != null)
            if (response.httpResponse.StatusCode == HttpStatusCode.OK && response.data != null)
            {
                if (response.data.Equals("All_Ok"))
                {
                    return (true, response.httpResponse);
                }
            }
        return (false, response.httpResponse);
    }
    /// <summary>
    /// Check if the Device is still reachable in this network.
    /// </summary>
    /// <returns></returns>
    public (bool connectionSuccess, HttpResponseMessage message) TestConnection()
    {
        return ApiClient.TryToConnectToNewDevice(this._httpRequestHelper);
    }
    
    /// <summary>
    /// Gets the Device Name
    /// </summary>
    /// <returns></returns>
    public (string? deviceName, HttpResponseMessage? message) GetDeviceName()
    {
        (HttpResponseMessage? httpResponse, string? data) response = _httpRequestHelper.HttpGetString("/DeviceInfo/GetDeviceName");
        if (response.httpResponse != null)
        {
            if (response.httpResponse.StatusCode == HttpStatusCode.OK && response.data != null)
            {
               return (response.data, response.httpResponse);
            }
        }
        return (null, response.httpResponse);
    }
    /// <summary>
    /// Sets the Device Name
    /// </summary>
    /// <param name="deviceName"></param>
    /// <returns></returns>
    public (bool success, HttpResponseMessage? message) SetDeviceName(string deviceName)
    {
        //_httpRequestHelper.HttpGetString($"/DeviceInfo/SetDeviceName?deviceName={deviceName}");
        /* HttpResponseMessage? response = _httpRequestHelper.HttpPost("/DeviceInfo/SetDeviceName", new FormUrlEncodedContent(
                     new Dictionary<string, string>()
                     {
                         { "deviceName", deviceName }
                     }));*/
        /*HttpResponseMessage? response = _httpRequestHelper.HttpPost("/DeviceInfo/SetDeviceName",
            new StringContent($"\"{deviceName}\"", Encoding.UTF8, "application/json"));*/
        HttpResponseMessage? response = _httpRequestHelper.HttpPost($"/DeviceInfo/SetDeviceName?deviceName={deviceName}", null);
        if (response != null)
        {
            if (response.StatusCode == HttpStatusCode.OK )
            {
                return (true, response);
            }
        }
        return (false, response);
    }
}