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
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SpotifyInterface
{
    public partial class Form1 : Form
    {
        private SpotifyWebAPI spotify;
        private PrivateProfile profile;
        private OpenFileDialog ofd = new OpenFileDialog();
        private ChromeOptions options = new ChromeOptions();
        private string filePath = "";

        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> tempList = new List<string>();

            if (fileNameTextBox.Text == "")
                MessageBox.Show("Please Select Valid File");
            else
            {
                if (fromFile.Checked == true)
                {
                    tempList = ReadIn(filePath);
                    CreatePlaylist(tempList);
                }

                if(fromMonitor.Checked == true)
                {
                    Boolean flag = false;
                    TimeSpan target = new TimeSpan(21, 0, 0); //target time to search is 9pm
                    button1.Enabled = false;
                    MessageBox.Show("Entering Monitor Mode");
                    while (!flag)
                    {
                        TimeSpan now = DateTime.Now.TimeOfDay;
                        if (now > target)
                        {
                            tempList = ReadIn(filePath);
                            CreatePlaylist(tempList);
                            flag = true;
                            button1.Enabled = true;
                        }
                    }
                }

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

        private async void RunAuthentication()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory(
                "http://localhost", 8000, "78c190180d5e4e79baf28a7ad4c04018",
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

        private List<string> ReadIn(string fileName)
        {
            List<string> tracks = new List<string>();
            MessageBox.Show("Reading");
            foreach (string line in File.ReadAllLines(fileName))
            {
                if(line != "")
                {
                    if (line.Contains("feat."))
                        tracks.Add(noFeat(line));
                    else if (line.Contains("&"))
                        tracks.Add(noAnd(line));
                    else
                        tracks.Add(line);
                }
            }

            return tracks;
        }

        public static string noAnd(String text)
        {
            StringBuilder sb = new StringBuilder();
            char[] content = text.ToCharArray();

            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == '&')
                    while (content[i] != '-')
                        i++;
                sb.Append(content[i]);
            }

            if (sb.ToString().Contains("feat."))
                return noFeat(sb.ToString());
            else
                return sb.ToString();
        }

        public static string noFeat(String text)
        {
            StringBuilder sb = new StringBuilder();
            char[] content = text.ToCharArray();
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == 'f' && content[i + 2] == 'e' && content[i + 3] == 'a' && content[i + 4] == 't')
                    while (content[i] != '-')
                        i++;
                sb.Append(content[i]);
            }

            if (sb.ToString().Contains("&"))
                return noAnd(sb.ToString());
            else
                return sb.ToString();
        }

        private void CreatePlaylist(List<string> tracks)
        {
            FullPlaylist newReleases = spotify.CreatePlaylist(profile.Id, DateTime.Now.ToString("MM/dd") + " Releases");
            SearchItem song = new SearchItem();
            ErrorResponse response = new ErrorResponse();

            if (newReleases.HasError()) //This might need more graceful integration
                Console.WriteLine(newReleases.Error.Message);

           foreach (string target in tracks)
            {
                if(target.Contains("EP") || target.Contains("Album") || target.Contains("Remixes"))
                {
                    song = spotify.SearchItems(target, SearchType.Album);
                    if (song.Albums.Total > 0)
                    {
                        FullAlbum album = spotify.GetAlbum(song.Albums.Items[0].Id);
                        for (int i = 0; i < album.Tracks.Total; i++)
                        {
                            response = spotify.AddPlaylistTrack(profile.Id, newReleases.Id, album.Tracks.Items[i].Uri);
                            playlistsListBox.Items.Add(album.Tracks.Items[i].Name);
                        }
                    }
                }
                else
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
            }

            MessageBox.Show("Playlist Created!");
            if (response.HasError()) //This might need more graceful integration
                Console.WriteLine(response.Error.Message);

        }

        private void test()
        {
            StringBuilder sb = new StringBuilder();
            //options.AddArguments("headless");
            options.AddArguments("disable-gpu");
            options.AddArguments("no-sandbox");
            IWebDriver browser = new ChromeDriver(options);

            browser.Navigate().GoToUrl(urlBox.Text);
            var results = browser.FindElement(By.CssSelector("#form-t3_8n84olz29 > div > div")).Text;
            foreach (char letter in results)
                sb.Append(letter);
            string test = sb.ToString();
            Console.WriteLine(test[0]);
        }
    }
}
