using System.Net;
using System.Net.Http;
using BoTech.StarClock.Client.Models.ApiClient;

namespace BoTech.StarClock.Client.Helper.ApiClient;

public class DeviceInfoApiClient(HttpRequestHelper httpRequestHelper)
{
    private HttpRequestHelper _httpRequestHelper = httpRequestHelper;
    /// <summary>
    /// Gets the Device Name
    /// </summary>
    /// <returns></returns>
    public RequestResult<string> GetDeviceName()
    {
        (HttpResponseMessage? httpResponse, string? data) response = _httpRequestHelper.HttpGetString("/DeviceInfo/GetDeviceName");
        if (response.httpResponse != null)
        {
            if (response.httpResponse.StatusCode == HttpStatusCode.OK && response.data != null)
            {
                return new RequestResult<string>(true, response.httpResponse, response.data);
            }
        }
        return new RequestResult<string>(false, response.httpResponse, null);
    }
    /// <summary>
    /// Sets the Device Name
    /// </summary>
    /// <param name="deviceName"></param>
    /// <returns></returns>
    public RequestResult<bool> SetDeviceName(string deviceName)
    {
        HttpResponseMessage? response = _httpRequestHelper.HttpPost($"/DeviceInfo/SetDeviceName?deviceName={deviceName}", null);
        if (response != null)
        {
            if (response.StatusCode == HttpStatusCode.OK )
            {
                return new RequestResult<bool>(true, response, true);
            }
        }
        return new RequestResult<bool>(false, response, false);
    }
}