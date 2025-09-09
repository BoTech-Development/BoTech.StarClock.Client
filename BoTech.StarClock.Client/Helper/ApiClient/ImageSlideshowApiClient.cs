using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using BoTech.StarClock.Api.SharedModels.Slideshow;
using BoTech.StarClock.Client.Models.ApiClient;
using Newtonsoft.Json;

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
        return _httpRequestHelper.HttpGetJsonObject<List<LocalImage>>("/ImageSlideshow/GetImageList");
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
        return _httpRequestHelper.HttpGetFile($"/ImageSlideshow/GetImagePreview?id={id}&width={width}&height={height}", fileName);
    }
    /// <summary>
    /// Uploads and Image to the device
    /// </summary>
    /// <param name="filename">The Filename of the Image</param>
    /// <returns>Among other infos the new id of the file</returns>
    public RequestResult<string> UploadImage(string filename)
    {
        RequestResult<dynamic> result = _httpRequestHelper.HttpPostFile($"/ImageSlideshow/UploadImage", filename);
        if (result.IsSuccess())
        {
            return new RequestResult<string>(true, result.ResponseMessage, result.ResponseMessage.Content.ReadAsStringAsync().Result, null);
        }
        return new RequestResult<string>(false, result.ResponseMessage, null, result.Error);
    }
    /// <summary>
    /// Uploads and Image to the device
    /// </summary>
    /// <param name="filename">The Filename of the Image with the path</param>
    /// <param name="content">The Content of the File.</param>
    /// <returns>Among other infos the new id of the file</returns>
    public RequestResult<string> UploadImage(string filename, byte[] content)
    {
        RequestResult<dynamic> result = _httpRequestHelper.HttpPostFile($"/ImageSlideshow/UploadImage",content, filename);
        if (result.IsSuccess())
        {
            return new RequestResult<string>(true, result.ResponseMessage, result.ResponseMessage.Content.ReadAsStringAsync().Result, null);
        }
        return new RequestResult<string>(false, result.ResponseMessage, null, result.Error);
    }
    /// <summary>
    /// Let the device delete an Image
    /// </summary>
    /// <param name="imageId">The id of the Image</param>
    /// <returns>Success or not.</returns>
    public RequestResult<dynamic> DeleteImage(Guid imageId)
    {
        return _httpRequestHelper.HttpDelete($"/ImageSlideshow/DeleteImage?id={imageId}");
    }
    /// <summary>
    /// Returns a list of all Images that are located in the Slidshow.
    /// </summary>
    /// <returns></returns>
    public RequestResult<List<LocalImage>> GetSlideshowImages()
    {
        return _httpRequestHelper.HttpGetJsonObject<List<LocalImage>>("/ImageSlideshow/GetSlideshowImages");
    }
    /// <summary>
    /// Deletes all Image refs from the Slideshow
    /// </summary>
    /// <returns></returns>
    public RequestResult<dynamic> ClearSlideshow()
    {
        return _httpRequestHelper.HttpDelete($"/ImageSlideshow/ClearSlideshow");
    }
    /// <summary>
    /// Deletes an Image from the Slideshow list
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public RequestResult<dynamic> DeleteImageFromSlideshow(int index)
    {
        return  _httpRequestHelper.HttpDelete($"/ImageSlideshow/DeleteImageFromSlideshow?index={index}");
    }
    /// <summary>
    /// Moves an Image through the slideshow Timeline.
    /// </summary>
    /// <param name="oldIndex">The old index of the Image</param>
    /// <param name="newIndex">The destination index</param>
    /// <returns></returns>
    public RequestResult<dynamic> MoveImageTo(int oldIndex, int newIndex)
    {
        return _httpRequestHelper.HttpPatch($"/ImageSlideshow/MoveImageTo?oldIndex={oldIndex}&newIndex={newIndex}", null);
    }
    /// <summary>
    /// Creates a duplicate of the Image and Inserts it into the Slideshow Image list
    /// </summary>
    /// <param name="indexOfImageToDuplicate">The old index of the Image</param>
    /// <returns></returns>
    public RequestResult<dynamic> DuplicateImage(int indexOfImageToDuplicate)
    {
        return _httpRequestHelper.HttpPatch($"/ImageSlideshow/DuplicateImage?indexOfImageToDuplicate={indexOfImageToDuplicate}", null);
    }
    /// <summary>
    /// Adds only one Image to the Slideshow
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RequestResult<dynamic> AddImageToSlideshow(string id)
    {
        return _httpRequestHelper.HttpPatch($"/ImageSlideshow/AddImageToSlideshow?id={id}", null);
    }
    /// <summary>
    /// Adds multiple Images to the Slideshow
    /// </summary>
    /// <param name="index"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    public RequestResult<dynamic> AddImageRangeToSlideshow(int index, string[] ids)
    {
        var jsonContent = JsonConvert.SerializeObject(ids);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        return _httpRequestHelper.HttpPatch($"/ImageSlideshow/AddImageToSlideshow?index={index}", content);
    }
}