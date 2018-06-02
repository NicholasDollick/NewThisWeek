using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using SpotifyAPI;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System.Windows.Forms;
using System.IO;

namespace SpotifyInterface
{
    public partial class Form1 : Form
    {
        private readonly ProxyConfig proxyConfig;
        private SpotifyWebAPI spotify;
        private PrivateProfile profile;
        private OpenFileDialog ofd = new OpenFileDialog();
        private string filePath = "";

        public Form1()
        {
            InitializeComponent();
            proxyConfig = new ProxyConfig();
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreatePlaylist();
            List<SimplePlaylist> test = GetPlaylists();

            
            foreach (SimplePlaylist list in test)
            {
                playlistsListBox.Items.Add(list.Name);
            }
        }

        private async void InitialSetup()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(InitialSetup));
                return;
            }

            authButton.Enabled = false;
            profile = await spotify.GetPrivateProfileAsync();
            nameLabel.Text = profile.DisplayName;
            countryLabel.Text = profile.Country;
            emailLabel.Text = profile.Email;
            accountLabel.Text = profile.Product;

            if(profile.Images != null && profile.Images.Count > 0)
            {
                using (WebClient wc = new WebClient())
                {
                    byte[] imageBytes = await wc.DownloadDataTaskAsync(new Uri(profile.Images[0].Url));
                    using (MemoryStream stream = new MemoryStream(imageBytes))
                        pictureBox.Image = System.Drawing.Image.FromStream(stream);
                }
            }

            button1.Enabled = true;
        }

        private List<SimplePlaylist> GetPlaylists()
        {
            Paging<SimplePlaylist> playlists = spotify.GetUserPlaylists(profile.Id);
            List<SimplePlaylist> list = playlists.Items.ToList();

            while (playlists.Next != null)
            {
                playlists = spotify.GetUserPlaylists(profile.Id, 20, playlists.Offset + playlists.Limit);
                list.AddRange(playlists.Items);
            }

            return list;
        }

        private void CreatePlaylist()
        {
            FullPlaylist newReleases = spotify.CreatePlaylist(profile.Id, DateTime.Now.ToString("MM/dd") + " Releases");
            
            if (!newReleases.HasError())
                MessageBox.Show("Playlist Created!");
            if (newReleases.HasError()) //This might need more graceful integration
                Console.WriteLine(newReleases.Error.Message);

            SearchItem song = spotify.SearchItems("Great Spirit - Armin van buuren", SearchType.Track);
            
            ErrorResponse response = spotify.AddPlaylistTrack(profile.Id, newReleases.Id, song.Tracks.Items[0].Uri);

            if (!response.HasError())
                MessageBox.Show("Track Added!");
            if (response.HasError()) //This might need more graceful integration
                Console.WriteLine(response.Error.Message);

        }

        private async void RunAuthentication()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory(
                "http://localhost", 8000, "spotify client id",
                Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate | Scope.UserLibraryRead |
                Scope.PlaylistModifyPublic | Scope.UserFollowRead | Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);

            try
            {
                spotify = await webApiFactory.GetWebApi();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (spotify == null)
                return;

            InitialSetup();
        }

        private void authButton_Click(object sender, EventArgs e)
        {
            Task.Run(() => RunAuthentication());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Text Files|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileNameTextBox.Text = ofd.SafeFileName;
                filePath = ofd.FileName;
            }
        }

        private void tempButton_Click(object sender, EventArgs e)
        {
            List<string> tempList = new List<string>();

            if (fileNameTextBox.Text == "")
                MessageBox.Show("Please Select Valid File");
            else
            {
                if(fromFile.Checked == true)
                    tempList = ReadIn(filePath);

                CreatePlaylist(tempList);
            }
        }

        private List<string> ReadIn(string fileName)
        {
            List<string> tracks = new List<string>();
            MessageBox.Show("Reading");
            foreach (string line in File.ReadAllLines(fileName))
            {
                if(line != "")
                    tracks.Add(line);
            }

            return tracks;
        }

        private void CreatePlaylist(List<string> tracks)
        {
            FullPlaylist newReleases = spotify.CreatePlaylist(profile.Id, DateTime.Now.ToString("MM/dd") + " Releases");
            SearchItem song = new SearchItem();
            ErrorResponse response = new ErrorResponse();

            //if (!newReleases.HasError())
               // MessageBox.Show("Playlist Created!");
            if (newReleases.HasError()) //This might need more graceful integration
                Console.WriteLine(newReleases.Error.Message);

           foreach (string target in tracks)
            {
                if(!target.Contains("EP"))
                {
                    song = spotify.SearchItems(target, SearchType.Track);
                    if (song.Tracks.Items.Count > 0)
                    {
                        response = spotify.AddPlaylistTrack(profile.Id, newReleases.Id, song.Tracks.Items[0].Uri);
                        playlistsListBox.Items.Add(song.Tracks.Items[0].Name);
                    }
                    if (response.HasError()) //This might need more graceful integration
                        Console.WriteLine(response.Error.Message);
                }
                else
                {
                    song = spotify.SearchItems(target, SearchType.Album);
                    if(song.Albums.Total > 0)
                    {
                        FullAlbum album = spotify.GetAlbum(song.Albums.Items[0].Id);
                        for(int i = 0; i < album.Tracks.Total; i++)
                            response = spotify.AddPlaylistTrack(profile.Id, newReleases.Id, album.Tracks.Items[i].Uri);
                    }
                }
            }
            // SearchItem song = spotify.SearchItems("Great Spirit - Armin van buuren", SearchType.Track);

            // ErrorResponse response = spotify.AddPlaylistTrack(profile.Id, newReleases.Id, song.Tracks.Items[0].Uri);
            MessageBox.Show("Playlist Created!");
            //if (!response.HasError())
                //MessageBox.Show("Track Added!");
            if (response.HasError()) //This might need more graceful integration
                Console.WriteLine(response.Error.Message);

        }
    }
}
