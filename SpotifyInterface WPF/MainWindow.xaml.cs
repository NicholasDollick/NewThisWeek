using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;

namespace SpotifyInterface_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpotifyWebAPI spotify;
        private PrivateProfile profile;
        private OpenFileDialog ofd = new OpenFileDialog();
        private string filePath = "";

        public MainWindow()
        {
            InitializeComponent();
            runButton.IsEnabled = false;
        }

        private async void InitialSetup()
        {
            if (!Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(() => InitialSetup());
                return;
            }

            authButton.IsEnabled = false;
            profile = await spotify.GetPrivateProfileAsync();
            userName.Text = profile.DisplayName;
            userCountry.Text = profile.Country;
            userEmail.Text = profile.Email;
            accountType.Text = profile.Product;

            if (profile.Images != null && profile.Images.Count > 0)
            {
                using (WebClient wc = new WebClient())
                {
                    byte[] imageBytes = await wc.DownloadDataTaskAsync(new Uri(profile.Images[0].Url));
                    userIcon.Source = ByteArrayToImage(imageBytes);
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
            runButton.IsEnabled = true;
        }

        public BitmapSource ByteArrayToImage(byte[] input)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(input);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        private async void RunAuthentication()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory(
                "http://localhost", 8000, "78c190180d5e4e79baf28a7ad4c04018",
                 Scope.UserReadEmail | Scope.PlaylistReadPrivate |
                Scope.PlaylistModifyPublic | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);

            try
            {
                spotify = await webApiFactory.GetWebApi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (spotify == null)
                return;

            InitialSetup();
        }

        private void authButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Run(() => RunAuthentication());
        }

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            ofd.Filter = "Text Files|*.txt";
            var result =  ofd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                fileNameTextBox.Text = ofd.SafeFileName;
                filePath = ofd.FileName;
            }
        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> tempList = new List<string>();

            if (fileNameTextBox.Text == "                       ")
                MessageBox.Show("Please Select Valid File");
            else
            {
                if (fromFile.IsChecked == true)
                {
                    tempList = ReadIn(filePath);
                    CreatePlaylist(tempList);
                }
                else
                    MessageBox.Show("Select Date Type");
            }
        }

        private List<string> ReadIn(string fileName)
        {
            List<string> tracks = new List<string>();
            MessageBox.Show("Reading");
            foreach (string line in File.ReadAllLines(fileName))
            {
                if (line != "")
                {
                    tracks.Add(CleanAndFormat(line));
                }
            }

            return tracks;
        }

        // Consider making this a single conditional checking through an array of phrases to remove
        public string CleanAndFormat(string track)
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

        private void CreatePlaylist(List<string> tracks)
        {
            FullPlaylist newReleases = spotify.CreatePlaylist(profile.Id, DateTime.Now.ToString("MM/dd") + " Releases");
            SearchItem song = new SearchItem();
            ErrorResponse response = new ErrorResponse();

            if (newReleases.HasError()) //This might need more graceful integration
                Console.WriteLine(newReleases.Error.Message);

            Mouse.OverrideCursor = Cursors.Wait;

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
                            playListBox.Items.Add(album.Tracks.Items[i].Name);
                        }
                    }
                }
                else
                {
                    song = spotify.SearchItems(target, SearchType.Track);
                    if (song.Tracks.Items.Count > 0)
                    {
                        response = spotify.AddPlaylistTrack(profile.Id, newReleases.Id, song.Tracks.Items[0].Uri);
                        playListBox.Items.Add(song.Tracks.Items[0].Name);
                    }

                    if (response.HasError()) //This might need more graceful integration
                        Console.WriteLine(response.Error.Message);
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;

            MessageBox.Show("Playlist Created!");
            if (response.HasError()) //This might need more graceful integration
                Console.WriteLine(response.Error.Message);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
