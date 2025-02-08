using Microsoft.AspNetCore.Mvc;
using YouTubeApiProject.Models;
using YouTubeApiProject.Services;

namespace YouTubeApiProject.Controllers
{
    public class YouTubeController : Controller
    {
        private readonly YouTubeApiService _youtubeService;
        private const int PageSize = 6;

        public YouTubeController(YouTubeApiService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        public IActionResult Index()
        {
            return View(); // Only show search bar
        }
        [HttpGet, HttpPost]
        public async Task<IActionResult> Search(string query, int page = 1, string dateFilter = "30")
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View("Index");
            }

            DateTime? publishedAfter = dateFilter switch
            {
                "7" => DateTime.UtcNow.AddDays(-7),
                "30" => DateTime.UtcNow.AddDays(-30),
                "90" => DateTime.UtcNow.AddDays(-90),
                _ => null
            };

            List<YouTubeVideoModel> videos;

            // Check if session exists, otherwise fetch new results
            if (HttpContext.Session.GetObject<List<YouTubeVideoModel>>("Videos") == null || page == 1)
            {
                videos = await _youtubeService.SearchVideosAsync(query, publishedAfter);
                HttpContext.Session.SetObject("Videos", videos);
            }
            else
            {
                videos = HttpContext.Session.GetObject<List<YouTubeVideoModel>>("Videos");
            }

            if (videos == null || !videos.Any())
            {
                ViewBag.Message = "No videos found.";
                return View("Index");
            }

            int totalVideos = videos.Count;
            int totalPages = (int)Math.Ceiling((double)totalVideos / PageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            var paginatedVideos = videos.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = query;
            ViewBag.SelectedDateFilter = dateFilter;

            return View("SearchResults", paginatedVideos); // Show search results in separate view
        }
    }
}
