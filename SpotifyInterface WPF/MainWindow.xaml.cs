﻿using SpotifyAPI.Web;
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
            authButton.IsEnabled = true;
        }

        private async void InitialSetup()
        {
            if (!Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(()=> InitialSetup());
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
                Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate | Scope.UserLibraryRead |
                Scope.PlaylistModifyPublic | Scope.UserFollowRead | Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);

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
            Nullable<bool> result =  ofd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                fileNameTextBox.Text = ofd.SafeFileName;
                filePath = ofd.FileName;
            }
        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> tempList = new List<string>();

            if (fileNameTextBox.Text == "")
                MessageBox.Show("Please Select Valid File");
            else
            {
                if (fromFile.IsChecked == true)
                {
                    tempList = ReadIn(filePath);
                    CreatePlaylist(tempList);
                }
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
