using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeApiProject.Models;

namespace YouTubeApiProject.Services
{
    public class YouTubeApiService
    {
        private readonly string _apiKey;

        public YouTubeApiService(IConfiguration configuration)
        {
            _apiKey = configuration["YouTubeApiKey"]; // Fetch API key from appsettings.json
        }

        // Search YouTube videos based on user query with published date filtering
        public async Task<List<YouTubeVideoModel>> SearchVideosAsync(string query, DateTime? publishedAfter = null)
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _apiKey,
                    ApplicationName = "YouTubeApiProject"
                });

                var searchRequest = youtubeService.Search.List("snippet");
                searchRequest.Q = query;
                searchRequest.MaxResults = 10;
                searchRequest.Type = "video";

                if (publishedAfter.HasValue)
                {
                    searchRequest.PublishedAfter = publishedAfter.Value.ToUniversalTime();
                }

                var searchResponse = await searchRequest.ExecuteAsync();

                var videos = searchResponse.Items
                    .Where(item => item.Id.VideoId != null)
                    .Select(item => new YouTubeVideoModel
                    {
                        Title = item.Snippet.Title,
                        ThumbnailUrl = item.Snippet.Thumbnails?.Medium?.Url,
                        VideoUrl = "https://www.youtube.com/watch?v=" + item.Id.VideoId,
                        PublishedAt = item.Snippet.PublishedAt,
                        ChannelTitle = item.Snippet.ChannelTitle
                    })
                    .ToList();

                return videos;
            }
            catch
            {
                return new List<YouTubeVideoModel>();
            }
        }

        // Get trending videos
        public async Task<List<YouTubeVideoModel>> GetTrendingVideosAsync()
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _apiKey,
                    ApplicationName = "YouTubeApiProject"
                });

                var videoRequest = youtubeService.Videos.List("snippet");
                videoRequest.Chart = VideosResource.ListRequest.ChartEnum.MostPopular;
                videoRequest.RegionCode = "US"; // Change if needed
                videoRequest.MaxResults = 10;

                var videoResponse = await videoRequest.ExecuteAsync();

                var videos = videoResponse.Items.Select(item => new YouTubeVideoModel
                {
                    Title = item.Snippet.Title,
                    ThumbnailUrl = item.Snippet.Thumbnails?.Medium?.Url,
                    VideoUrl = "https://www.youtube.com/watch?v=" + item.Id,
                    PublishedAt = item.Snippet.PublishedAt,
                    ChannelTitle = item.Snippet.ChannelTitle
                }).ToList();

                return videos;
            }
            catch
            {
                return new List<YouTubeVideoModel>();
            }
        }
    }
}
