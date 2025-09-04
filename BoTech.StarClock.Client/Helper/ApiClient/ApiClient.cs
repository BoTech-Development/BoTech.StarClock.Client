using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using BoTech.StarClock.Api.SharedModels.Slideshow;
using BoTech.StarClock.Client.Models.ApiClient;

namespace BoTech.StarClock.Client.Helper.ApiClient;

public class ApiClient
{
    /// <summary>
    /// The Client for the ImageSlideshow Controller.
    /// </summary>
    public ImageSlideshowApiClient ImageSlideshowClient { get; }
    /// <summary>
    /// The Client for the DeviceInfo Controller.
    /// </summary>
    public DeviceInfoApiClient DeviceInfoClient { get; }
    private HttpRequestHelper _httpRequestHelper;

    private ApiClient(HttpRequestHelper httpRequestHelper)
    {
        _httpRequestHelper = httpRequestHelper;
        ImageSlideshowClient = new ImageSlideshowApiClient(_httpRequestHelper);
        DeviceInfoClient = new DeviceInfoApiClient(_httpRequestHelper);
    }
    /// <summary>
    /// Creates an ApiClient, which can be used to access a specific api, which is hosted on a specific IpAddress
    /// </summary>
    /// <param name="baseUrl">the url of the api</param>
    /// <returns>The instance of the client when a connection can be established and the error or success message.</returns>
    public static (ApiClient? client, HttpResponseMessage? message) CreateApiClientFor(string baseUrl)
    {
        HttpRequestHelper requestHelper = new HttpRequestHelper(baseUrl);
        RequestResult<bool> result = TryToConnectToNewDevice(requestHelper);
        if(result.ParsedData) return (new ApiClient(requestHelper), result.ResponseMessage);
        return (null, result.ResponseMessage);
    }
    /// <summary>
    /// Check if the given Device is still reachable in this network.
    /// </summary>
    /// <param name="requestHelper"></param>
    /// <returns></returns>
    private static RequestResult<bool> TryToConnectToNewDevice(
        HttpRequestHelper requestHelper)
    {
        (HttpResponseMessage? httpResponse, string? data) response = requestHelper.HttpGetString("/Status/TestConnection");
        if(response.httpResponse != null)
            if (response.httpResponse.StatusCode == HttpStatusCode.OK && response.data != null)
            {
                if (response.data.Equals("All_Ok"))
                {
                    return new RequestResult<bool>(true, response.httpResponse, true);
                }
            }
        return new RequestResult<bool>(false, response.httpResponse, false);
    }
    /// <summary>
    /// Check if the Device is still reachable in this network.
    /// </summary>
    /// <returns></returns>
    public RequestResult<bool> TestConnection()
    {
        RequestResult<bool> result = ApiClient.TryToConnectToNewDevice(this._httpRequestHelper);
        return new RequestResult<bool>(result.ParsedData, result.ResponseMessage, result.ParsedData);
    }
}