using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using YouTubeApiProject.Services;
using System.Collections.Generic;
using YouTubeApiProject.Models;

namespace YouTubeApiProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly YouTubeApiService _youtubeService;

        public HomeController(ILogger<HomeController> logger, YouTubeApiService youtubeService)
        {
            _logger = logger;
            _youtubeService = youtubeService;
        }

        public async Task<IActionResult> Index()
        {
            var trendingVideos = await _youtubeService.GetTrendingVideosAsync();
            return View(trendingVideos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

