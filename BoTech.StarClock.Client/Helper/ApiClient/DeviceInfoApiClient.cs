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
        return _httpRequestHelper.HttpGetString("/DeviceInfo/GetDeviceName");
    }
    /// <summary>
    /// Sets the Device Name
    /// </summary>
    /// <param name="deviceName"></param>
    /// <returns></returns>
    public RequestResult<dynamic> SetDeviceName(string deviceName)
    {
        return _httpRequestHelper.HttpPost($"/DeviceInfo/SetDeviceName?deviceName={deviceName}", null);
    }
}