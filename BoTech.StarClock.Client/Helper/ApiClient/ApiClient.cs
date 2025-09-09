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
    public static (ApiClient client, HttpResponseMessage? message) CreateApiClientFor(string baseUrl)
    {
        HttpRequestHelper requestHelper = new HttpRequestHelper(baseUrl);
        RequestResult<bool> result = TryToConnectToNewDevice(requestHelper);
        return (new ApiClient(requestHelper), result.ResponseMessage);
    }
    /// <summary>
    /// Check if the given Device is still reachable in this network.
    /// </summary>
    /// <param name="requestHelper"></param>
    /// <returns></returns>
    private static RequestResult<bool> TryToConnectToNewDevice(
        HttpRequestHelper requestHelper)
    {
        RequestResult<string> response = requestHelper.HttpGetString("/Status/TestConnection");
        if(response.IsSuccess())
            if (response.ResponseMessage.Content.ReadAsStringAsync().Result.Equals("All_Ok"))
            {
                return new RequestResult<bool>(true, response.ResponseMessage, true, null);
            }
        return new RequestResult<bool>(false, response.ResponseMessage, false, response.Error);
    }
    /// <summary>
    /// Check if the Device is still reachable in this network.
    /// </summary>
    /// <returns></returns>
    public RequestResult<bool> TestConnection()
    {
        return ApiClient.TryToConnectToNewDevice(this._httpRequestHelper);
    }
}