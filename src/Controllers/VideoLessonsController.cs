using ElbekningC_Darslari.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;

namespace ElbekningC_Darslari.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoLessonsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetVideoLessons()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyD9NQhpkdvapp_ZRDR-Mfkg-guGLAxBCfQ",
                ApplicationName = "Elbekning C# Darslari"
            });

            var searchRequest = youtubeService.Search.List("snippet");
            searchRequest.ChannelId = "UCWDF6TvAUR2NZsuljGO-i5A";
            searchRequest.Order = SearchResource.ListRequest.OrderEnum.Title;
            searchRequest.MaxResults = 50;

            var searchResponse = await searchRequest.ExecuteAsync();

            var list = searchResponse.Items.Select(item => new VideoDetails
            {
                Title = item.Snippet.Title,
                Link = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
                Thumbnail = item.Snippet.Thumbnails.Medium.Url,
                PublishedAt = item.Snippet.PublishedAtDateTimeOffset
            })
            .OrderByDescending(video => video.PublishedAt).ToList();

            return Ok(list);
        }
    }
}
