using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace BoTech.StarClock.Client.Helper;

public class HttpRequestHelper
{
    private string _baseUrl;
    public HttpRequestHelper(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public HttpResponseMessage? HttpGetFile(string url, string fileName)
    {
        using (HttpClient client = new HttpClient())
        {
            client.BaseAddress = new Uri(_baseUrl);
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            response.EnsureSuccessStatusCode();

            using (Stream stream = response.Content.ReadAsStreamAsync().Result)
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
            Console.WriteLine($"File downloaded to: {fileName}");
            return response;
        }
    }
    public (HttpResponseMessage? httpResponse, T? data) HttpGetJsonObject<T>(string url)
    {
        HttpResponseMessage? response = HttpGet(url);
        if (response != null)
        {
            string jsonData = response.Content.ReadAsStringAsync().Result;
            return (response, JsonConvert.DeserializeObject<T>(jsonData));
        }
        return (null, default(T));
    }
    /// <summary>
    /// Sends a request to _baseUrl + url and returns the string returned by that method.
    /// </summary>
    /// <param name="url"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The returned string from the api or the </returns>
    public (HttpResponseMessage? httpResponse, string? data) HttpGetString(string url)
    {
        HttpResponseMessage? response = HttpGet(url);
        if (response != null)
        {
           return (response, response.Content.ReadAsStringAsync().Result);
        }
        return (null, null);
    }
    /// <summary>
    /// Sends a request to _baseUrl + url and returns the http response.
    /// </summary>
    /// <param name="url">The Api url</param>
    /// <returns>The result of the Get request.</returns>
    private HttpResponseMessage? HttpGet(string url)
    {
        if (_baseUrl != String.Empty)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                try
                {
                    Console.Write($"Performing GET request: {_baseUrl + url}");
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();
                    
                    Console.WriteLine($" GET Response Status: { response.StatusCode } ");
                    return response;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($" GET Request error: {e.Message}");
                }
            }
        }
        return null;
    }
    public HttpResponseMessage? HttpPatch(string url, HttpContent content)
    {
        if (_baseUrl != String.Empty)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                try
                {
                    Console.Write($"Performing Patch request: {_baseUrl + url}");
                    HttpResponseMessage response = client.PatchAsync(url, content).Result;

                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();
                    
                    Console.WriteLine($" Patch Response Status: { response.StatusCode } ");
                    return response;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($" Patch Request error: {e.Message}");
                }
            }
        }
        return null;
    }
    /// <summary>
    /// Uploads a file from path to the web api.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public HttpResponseMessage? HttpPostFile(string url, string fileName)
    {
        if (File.Exists(fileName))
        {
            using (var form = new MultipartFormDataContent())
            {
                // Read the file content
                var fileContent = new ByteArrayContent(File.ReadAllBytesAsync(fileName).Result);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

                // Add the file to the form data
                form.Add(fileContent, "file", Path.GetFileName(fileName));
                return HttpPost(url, form);
            }
        }
        return null;
    }
    /// <summary>
    /// This method executes an HttpPost request to the given endpoint. <br/>
    /// If you like to use url param you can inject them into the url string.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public HttpResponseMessage? HttpPost(string url, HttpContent? content)
    {
        if (_baseUrl != String.Empty)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                try
                {
                    Console.Write($"Performing Post request: {_baseUrl + url}");
                    HttpResponseMessage response = client.PostAsync(url, content).Result;

                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();
                    
                    Console.WriteLine($" Post Response Status: { response.StatusCode } ");
                    return response;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($" Post Request error: {e.Message}");
                }
            }
        }
        return null;
    }
    /// <summary>
    /// This method executes an HttpDelete request to the given endpoint. <br/>
    /// If you like to use url param you can inject them into the url string.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public HttpResponseMessage? HttpDelete(string url)
    {
        if (_baseUrl != String.Empty)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                try
                {
                    Console.Write($"Performing Delete request: {_baseUrl + url}");
                    HttpResponseMessage response = client.DeleteAsync(url).Result;

                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();
                    
                    Console.WriteLine($" Delete Response Status: { response.StatusCode } ");
                    return response;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($" Delete Request error: {e.Message}");
                }
            }
        }
        return null;
    }
}