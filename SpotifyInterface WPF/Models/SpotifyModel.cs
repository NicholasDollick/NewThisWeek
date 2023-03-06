using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System.Windows.Media.Imaging;

namespace SpotifyInterface_WPF.Models
{
    class SpotifyModel
    {
        public PrivateProfile Profile { get; set; }
        public SpotifyWebAPI Spotify { get; set; }
        public string DisplayName { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Product { get; set; }
        public BitmapImage Icon { get; set; }
    }
}
