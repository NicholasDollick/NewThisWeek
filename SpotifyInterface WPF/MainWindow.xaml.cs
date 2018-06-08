using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace SpotifyInterface_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpotifyWebAPI spotify;
        private PrivateProfile profile;

        public MainWindow()
        {
            InitializeComponent();
            authButton.IsEnabled = true;
        }

        private void authButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Run(() => RunAuthentication());
            playListBox.Items.Add("test");
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

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
