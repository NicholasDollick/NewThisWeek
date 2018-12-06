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

namespace SpotifyInterface_WPF
{
    public class Logic : INotifyPropertyChanged
    {
        private BindableCollection<SongModel> _songs = new BindableCollection<SongModel>();
        private SongModel _selectedSong;

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

        public void test(SynchronizationContext _thread, BindableCollection<SongModel> songs)
        {
            while (true)
            {
                _thread.Send((object state) => {
                    songs.Add(new SongModel() { SongTitle = "test" });
                }, null);
            }
        }


        private void CreatePlaylist(SynchronizationContext mainThread, BindableCollection<SongModel> songs, SpotifyWebAPI spotify, PrivateProfile profile, List<string> tracks)
        {
            FullPlaylist newReleases = spotify.CreatePlaylist(profile.Id, DateTime.Now.ToString("MM/dd") + " Releases");
            SearchItem song = new SearchItem();
            ErrorResponse response = new ErrorResponse();

            if (newReleases.HasError()) //This might need more graceful integration
                Console.WriteLine(newReleases.Error.Message);

            // Passes cursor update to main thread
            //mainThread.Post((object state) => { Mouse.OverrideCursor = Cursors.Wait; Status.Text = "Status: Searching"; }, null);


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

                            // Passes listbox ui update to main thread
                            mainThread.Send((object state) => {
                              songs.Add(new SongModel() { SongTitle = album.Tracks.Items[i].Name });
                            }, null);
                        }
                    }
                }
                else
                {
                    song = spotify.SearchItems(target, SearchType.Track);
                    if (song.Tracks.Items.Count > 0)
                    {
                        response = spotify.AddPlaylistTrack(profile.Id, newReleases.Id, song.Tracks.Items[0].Uri);

                        // Passes listbox ui update to main thread
                        mainThread.Send((object state) => {
                            songs.Add(new SongModel() { SongTitle = song.Tracks.Items[0].Name });
                        }, null);
                    }

                    if (response.HasError()) //This might need more graceful integration
                        Console.WriteLine(response.Error.Message);
                }


                // pass percent complete updates to main thread
                /*
                mainThread.Send((object state) => {
                    percentDone += counter;
                    progressBar.Value = percentDone;
                    Amount.Text = percentDone.ToString("0.") + "%";
                }, null);
                */
            }

            //MessageBox.Show("Playlist Created!");
            if (response.HasError()) //This might need more graceful integration
                Console.WriteLine(response.Error.Message);

        }
    }
}
