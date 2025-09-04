using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using BoTech.StarClock.Api.SharedModels.Slideshow;
using BoTech.StarClock.Client.Models.ApiClient;

namespace BoTech.StarClock.Client.Helper.ApiClient;

public class ImageSlideshowApiClient(HttpRequestHelper httpRequestHelper)
{
    private HttpRequestHelper _httpRequestHelper = httpRequestHelper;
    /// <summary>
    /// Returns a List of all Images that are uploaded on the device.
    /// </summary>
    /// <returns></returns>
    public RequestResult<List<LocalImage>> GetImageList()
    {
        (HttpResponseMessage? httpResponse, List<LocalImage>? images) = _httpRequestHelper.HttpGetJsonObject<List<LocalImage>>("/ImageSlideshow/GetImageList");
        if (httpResponse != null)
        {
            if (httpResponse.StatusCode == HttpStatusCode.OK && images != null)
            {
                return new RequestResult<List<LocalImage>>(true, httpResponse, images);
            }
        }
        return new RequestResult<List<LocalImage>>(false, httpResponse, null);
    }
    /// <summary>
    /// Saves the Image Preview to a local path. The Image will be resized on the server.
    /// </summary>
    /// <param name="id">The Id of the Uploaded Image</param>
    /// <param name="width">Width of the resized Image</param>
    /// <param name="height">Height of the resized Image</param>
    /// <param name="fileName">The full Path to the destination file, including the filename and its extension.</param>
    /// <returns> a request result, for determining if the request was successful.</returns>
    public RequestResult<dynamic> GetImagePreview(string id, int width, int height, string fileName)
    {
        HttpResponseMessage? httpResponse = _httpRequestHelper.HttpGetFile($"/ImageSlideshow/GetImagePreview?id={id}&width={width}&height={height}", fileName);
        if (httpResponse != null)
        {
            if (httpResponse.StatusCode == HttpStatusCode.OK )
            {
                return new RequestResult<dynamic>(true, httpResponse, null);
            }
        }
        return new RequestResult<dynamic>(false, httpResponse, null);
    }
    /// <summary>
    /// Uploads and Image to the device
    /// </summary>
    /// <param name="filename">The Filename of the Image</param>
    /// <returns>Among other infos the new id of the file</returns>
    public RequestResult<string> UploadImage(string filename)
    {
        HttpResponseMessage? httpResponse = _httpRequestHelper.HttpPostFile($"/ImageSlideshow/UploadImage", filename);
        if (httpResponse != null)
        {
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return new RequestResult<string>(true, httpResponse, httpResponse.Content.ReadAsStringAsync().Result);
            }
        }
        return new RequestResult<string>(false, httpResponse, null);
    }
    /// <summary>
    /// Let the device delete an Image
    /// </summary>
    /// <param name="imageId">The id of the Image</param>
    /// <returns>Success or not.</returns>
    public RequestResult<bool> DeleteImage(Guid imageId)
    {
        HttpResponseMessage? httpResponse = _httpRequestHelper.HttpDelete($"/ImageSlideshow/DeleteImage?id={imageId}");
        if (httpResponse != null)
        {
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return new RequestResult<bool>(true, httpResponse, true);
            }
        }
        return new RequestResult<bool>(false, httpResponse, false);
    }
}