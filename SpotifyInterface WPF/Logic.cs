using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using Caliburn.Micro;
using SpotifyInterface_WPF.Models;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Net;

namespace SpotifyInterface_WPF
{
    public class Logic : INotifyPropertyChanged
    {
        private SpotifyWebAPI _spotify;
        private PrivateProfile _profile;
        private FullPlaylist _newReleases;
        private SearchItem _song;
        private ErrorResponse _response;

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(Func<object> song)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("song"));
        }

        public List<string> ReadIn(string fileName)
        {
            List<string> tracks = new List<string>();
            //Status.Text = "Status: Reading";
            foreach (string line in File.ReadAllLines(fileName))
            {
                if (line != "")
                {
                    tracks.Add(CleanAndFormat(line));
                }
            }

            return tracks;
        }

        private static string CleanAndFormat(string track)
        {
            string[] terms = { "feat.", "ft.", "&" };

            foreach (string term in terms)
            {
                if (track.Contains(term))
                {
                    int start = track.IndexOf(term);
                    int end = track.IndexOf("-");

                    if (start > end) //this loop would be if term comes after the - in line of text (ie: Artist - Title feat. Artist)
                        track = track.Substring(0, start);
                    else
                        track = track.Substring(0, start) + (track.Substring(end, (track.Length - end)));
                }
            }

            return track;
        }

        public SongModel test(string thing)
        {
            //return new SongModel() { SongTitle = "TEST" };

            return PopulatePlaylist(thing);
        }

        public void InitPlaylist()
        {
            _newReleases = _spotify.CreatePlaylist(_profile.Id, DateTime.Now.ToString("MM/dd") + " Releases");
            _song = new SearchItem();
            _response = new ErrorResponse();

            if (_newReleases.HasError())
                Console.WriteLine(_newReleases.Error.Message);
        }

        public SongModel PopulatePlaylist(string target)
        {

            if (target.Contains("EP") || target.Contains("Album") || target.Contains("Remixes"))
            {
                _song = _spotify.SearchItems(target, SearchType.Album);
                if (_song.Albums.Total > 0)
                {
                    FullAlbum album = _spotify.GetAlbum(_song.Albums.Items[0].Id);
                    List<SongModel> albumtracks = new List<SongModel>();

                    foreach (var track in album.Tracks.Items)
                    {
                        _response = _spotify.AddPlaylistTrack(_profile.Id, _newReleases.Id, track.Uri);
                         albumtracks.Add(new SongModel() { SongTitle = track.Name });
                    }
                    AlbumReturn(albumtracks);
                }
            }
            else
            {
                _song = _spotify.SearchItems(target, SearchType.Track);
                if (_song.Tracks.Items.Count > 0)
                {
                    _response = _spotify.AddPlaylistTrack(_profile.Id, _newReleases.Id, _song.Tracks.Items[0].Uri);

                    return (new SongModel() { SongTitle = _song.Tracks.Items[0].Name });
                }

                if (_response.HasError())
                    Console.WriteLine(_response.Error.Message);
            }

            if (_response.HasError())
                Console.WriteLine(_response.Error.Message);


            return new SongModel() { SongTitle = "" };
        }

        private IEnumerable<SongModel> AlbumReturn(List<SongModel> test)
        {
            foreach (var thing in test)
                yield return thing;
        }

        public async void Auth()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory(
                "http://localhost", 8000, "78c190180d5e4e79baf28a7ad4c04018", Scope.UserReadEmail | Scope.PlaylistReadPrivate |
                Scope.PlaylistModifyPublic | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);

            try
            {
                _spotify = await webApiFactory.GetWebApi();
                _profile = _spotify.GetPrivateProfile();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (_spotify == null)
            {
                return;
            }
        }

        public string GetDisplayName()
        {
            return _profile.DisplayName;
        }

        public string GetCountry()
        {
            return _profile.Country;
        }

        public string GetEmail()
        {
            return _profile.Email;
        }

        public string GetProduct()
        {
            return _profile.Product;
        }

        public BitmapImage GetImage()
        {
            if (_profile.Images != null && _profile.Images.Count > 0)
            {
                using (WebClient wc = new WebClient())
                {
                    byte[] imageBytes = wc.DownloadData(new Uri(_profile.Images[0].Url));
                    return (BitmapImage)ByteArrayToImage(imageBytes); //this is a spicy cast
                }
            }

            return null;
        }

        private BitmapSource ByteArrayToImage(byte[] input)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(input);
            bitmapImage.EndInit();
            return bitmapImage;
        }

    }
}
