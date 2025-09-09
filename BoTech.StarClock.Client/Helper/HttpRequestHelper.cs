using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive;
using BoTech.StarClock.Client.Models.ApiClient;
using Newtonsoft.Json;

namespace BoTech.StarClock.Client.Helper;

public class HttpRequestHelper
{
    private string _baseUrl;
    public HttpRequestHelper(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public RequestResult<dynamic> HttpGetFile(string url, string fileName)
    {
        using (HttpClient client = new HttpClient())
        {
            try
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
                return new RequestResult<dynamic>(true, response, fileName, null);
            }
            catch (Exception e)
            {
                return new RequestResult<dynamic>(false, null, null, e);
            }
        }
    }
    public RequestResult<T> HttpGetJsonObject<T>(string url)
    {
        RequestResult<dynamic> response = HttpGet(url);
        if (response.IsSuccess())
        {
            string jsonData = response.ResponseMessage!.Content.ReadAsStringAsync().Result;
            return new RequestResult<T>(true, response.ResponseMessage, JsonConvert.DeserializeObject<T>(jsonData), null);
        }
        return new RequestResult<T>(false, response.ResponseMessage, default(T), response.Error);
    }
    /// <summary>
    /// Sends a request to _baseUrl + url and returns the string returned by that method.
    /// </summary>
    /// <param name="url">The endpoint</param>
    /// <returns>The returned string from the api or the </returns>
    public RequestResult<string> HttpGetString(string url)
    {
        RequestResult<dynamic> response = HttpGet(url);
        if (response.IsSuccess())
        {
            string data = response.ResponseMessage!.Content.ReadAsStringAsync().Result;
            return new RequestResult<string>(true, response.ResponseMessage, data, null);
        }
        return new RequestResult<string>(false, response.ResponseMessage, string.Empty, response.Error);
    }
    /// <summary>
    /// Sends a request to _baseUrl + url and returns the http response.
    /// </summary>
    /// <param name="url">The Api url</param>
    /// <returns>The result of the Get request.</returns>
    private RequestResult<dynamic> HttpGet(string url)
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
                return new RequestResult<dynamic>(true, response, null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($" GET Request error: {e.Message}");
                return new RequestResult<dynamic>(false, null, null, e);
            }
        }
    }
    public RequestResult<dynamic> HttpPatch(string url, HttpContent? content)
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
                return new RequestResult<dynamic>(true, response, null, null);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($" Patch Request error: {e.Message}");
                return new RequestResult<dynamic>(true, null, null, e);
            }
        }
    }
    /// <summary>
    /// Uploads a file from path to the web api.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public RequestResult<dynamic> HttpPostFile(string url, string fileName)
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
        return new  RequestResult<dynamic>(false, null, null, new ArgumentException("File does not exist"));
    }
    /// <summary>
    /// Uploads a file from path to the web api.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="fileName">The name of the File with the path</param>
    /// <param name="content">The Content of the File.</param>
    /// <returns></returns>
    public RequestResult<dynamic> HttpPostFile(string url, byte[] content,  string fileName)
    {
        using (var form = new MultipartFormDataContent())
        {
            // Read the file content
            var fileContent = new ByteArrayContent(content);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

            // Add the file to the form data
            form.Add(fileContent, "file", Path.GetFileName(fileName));
            return HttpPost(url, form);
        }
    }
    /// <summary>
    /// This method executes an HttpPost request to the given endpoint. <br/>
    /// If you like to use url param you can inject them into the url string.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public RequestResult<dynamic> HttpPost(string url, HttpContent? content)
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
                return new RequestResult<dynamic>(true, response, null, null);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($" Post Request error: {e.Message}");
                return new RequestResult<dynamic>(false, null, null, e);
            }
        }
    }
    /// <summary>
    /// This method executes an HttpDelete request to the given endpoint. <br/>
    /// If you like to use url param you can inject them into the url string.
    /// </summary>
    /// <param name="url">The api endpoint</param>
    /// <returns></returns>
    public RequestResult<dynamic> HttpDelete(string url)
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
                return new RequestResult<dynamic>(true, response, null, null);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($" Delete Request error: {e.Message}");
                return new RequestResult<dynamic>(false, null, null, e);
            }
        }
    }
}