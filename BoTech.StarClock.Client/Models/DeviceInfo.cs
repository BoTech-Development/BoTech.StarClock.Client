
using BoTech.StarClock.Client.Helper;
using Newtonsoft.Json;

namespace BoTech.StarClock.Client.Models;

public class DeviceInfo (string ipAddress, string deviceName)
{
    /// <summary>
    /// The Local IpAddress of the Star
    /// </summary>
    public string IpAddress { get; set; } = ipAddress;
    /// <summary>
    /// The name that the user gives the Star.
    /// </summary>
    public string DeviceName { get; set; } = deviceName;
    /// <summary>
    /// Each device gets its own Client, to make it easier => Client has the base address already injected.
    /// </summary>
    [JsonIgnore]
    public ApiClient ApiClient { get; set; }
}