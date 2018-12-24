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
using SpotifyInterface_WPF.Models;

namespace SpotifyInterface_WPF
{
    public class Logic : INotifyPropertyChanged
    {
        private SpotifyWebAPI _spotify;
        private PrivateProfile _profile;
        public static BindableCollection<SongModel> _songs = new BindableCollection<SongModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public BindableCollection<SongModel> Songs
        {
            get { return _songs; }
            set { _songs = value; }
        }

        private void OnPropertyChanged(Func<object> song)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("song"));
        }

        private List<string> ReadIn(string fileName)
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

        public static string CleanAndFormat(string track)
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

        public void hello()
        {
            _songs.Add(new SongModel() { SongTitle = "bitch" });

        }

        public void test(BindableCollection<SongModel> songs)
        {
            while (true)
            {
                songs.Add(new SongModel() { SongTitle = "test" });
            }
        }


        public void CreatePlaylist(BindableCollection<SongModel> songs, SpotifyWebAPI spotify, PrivateProfile profile, List<string> tracks)
        {
            FullPlaylist newReleases = spotify.CreatePlaylist(profile.Id, DateTime.Now.ToString("MM/dd") + " Releases");
            SearchItem song = new SearchItem();
            ErrorResponse response = new ErrorResponse();

            if (newReleases.HasError()) //This might need more graceful integration
                Console.WriteLine(newReleases.Error.Message);


            foreach (string target in tracks)
            {
                if (target.Contains("EP") || target.Contains("Album") || target.Contains("Remixes"))
                {
                    song = spotify.SearchItems(target, SearchType.Album);
                    if (song.Albums.Total > 0)
                    {
                        FullAlbum album = spotify.GetAlbum(song.Albums.Items[0].Id);
                        for (int i = 0; i < album.Tracks.Total; i++)
                        {
                            response = spotify.AddPlaylistTrack(profile.Id, newReleases.Id, album.Tracks.Items[i].Uri);

                            songs.Add(new SongModel() { SongTitle = album.Tracks.Items[i].Name });
                        }
                    }
                }
                else
                {
                    song = spotify.SearchItems(target, SearchType.Track);
                    if (song.Tracks.Items.Count > 0)
                    {
                        response = spotify.AddPlaylistTrack(profile.Id, newReleases.Id, song.Tracks.Items[0].Uri);

                        songs.Add(new SongModel() { SongTitle = song.Tracks.Items[0].Name });
                    }

                    if (response.HasError()) //This might need more graceful integration
                        Console.WriteLine(response.Error.Message);
                }
            }

            if (response.HasError()) //This might need more graceful integration
                Console.WriteLine(response.Error.Message);


        }

        public async void Auth()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory(
                "http://localhost", 8000, "78c190180d5e4e79baf28a7ad4c04018", Scope.UserReadEmail | Scope.PlaylistReadPrivate |
                Scope.PlaylistModifyPublic | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);

            try
            {
                _spotify = await webApiFactory.GetWebApi();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (_spotify == null)
            {
                return;
            }

            InitialSetup();
        }
    }
}
