namespace YouTubeApiProject.Models
{
    public class YouTubeVideoModel
    {
        public string Title { get; set; }
        public string ChannelTitle { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime? PublishedAt { get; set; }

        // New property for duration in seconds
        public int DurationInSeconds { get; set; }

        // Formatted PublishedAt date for view
        public string PublishedAtFormatted => PublishedAt?.ToString("MMMM dd, yyyy") ?? "Unknown";
    }
}
