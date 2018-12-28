using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
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
