using System.Net.Http;

namespace BoTech.StarClock.Client.Models.ApiClient;

public class RequestResult<T>(bool success, HttpResponseMessage? message, T? data)
{
    /// <summary>
    /// The response Message from the http request
    /// </summary>
    public HttpResponseMessage? ResponseMessage { get; set; } = message;
    /// <summary>
    /// Data object which has been parsed from the http request
    /// </summary>
    public T? ParsedData { get; set; } = data;
    /// <summary>
    /// When the request was successful this var will be true.
    /// </summary>
    public bool Success { get; set; } = success;
}