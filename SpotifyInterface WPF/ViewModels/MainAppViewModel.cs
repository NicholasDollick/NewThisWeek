using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyInterface_WPF.Models;
using Caliburn.Micro;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web;
using System.Windows;
using System.Windows.Threading;
using SpotifyAPI.Web.Models;
using System.Net;
using System.Windows.Media.Imaging;
using System.IO;

namespace SpotifyInterface_WPF.ViewModels
{
    public class MainAppViewModel : Screen
    { 
        private string _userName = "-";
        private string _userCountry = "-";
        private string _userEmail = "-";
        private string _userAccountType = "-";
        private SpotifyWebAPI _spotify;
        private PrivateProfile _profile;
        private static string DefaultImage = "https://i.imgur.com/8IHaKKE.png";
        private BitmapImage icon = new BitmapImage(new Uri(DefaultImage, UriKind.Absolute));
        
        public string UserName
        {
            get { return _userName;  }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        public string UserCountry
        {
            get { return _userCountry; }
            set
            {
                _userCountry = value;
                NotifyOfPropertyChange(() => UserCountry);
            }
        }

        public string UserEmail
        {
            get { return _userEmail; }
            set
            {
                _userEmail = value;
                NotifyOfPropertyChange(() => UserEmail);
            }
        }

        public string UserAccountType
        {
            get { return _userAccountType; }
            set
            {
                _userAccountType = value;
                NotifyOfPropertyChange(() => UserAccountType);
            }
        }

        public BitmapImage DisplayedImage
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
                NotifyOfPropertyChange(() => DisplayedImage);
            }
        }

        public bool CanAuthSession(string userName)
        {
            if (userName.Equals("-"))
                return true;
            else
                return false;
        }

        public async void AuthSession(string userName)
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
                MessageBox.Show(ex.Message);
            }

            if(_spotify == null)
            {
                return;
            }

            InitialSetup();
        }

        private async void InitialSetup()
        { 
            _profile = await _spotify.GetPrivateProfileAsync();
            UserName = _profile.DisplayName;
            UserCountry = _profile.Country;
            UserEmail = _profile.Email;
            UserAccountType = _profile.Product;

            if (_profile.Images != null && _profile.Images.Count > 0)
            {
                using (WebClient wc = new WebClient())
                {
                    byte[] imageBytes = await wc.DownloadDataTaskAsync(new Uri(_profile.Images[0].Url));
                    icon = (BitmapImage)ByteArrayToImage(imageBytes); //this is a spicy cast
                    NotifyOfPropertyChange(() => DisplayedImage);
                }
            }
        }
        
        private BitmapSource ByteArrayToImage(byte[] input)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(input);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public bool CanRunLogic(string userName)
        {
            if (userName.Equals("-"))
                return false;
            else
                return true;
        }

        public void RunLogic(string userName)
        {

        }

        public void Close() // this doesnt do what it seems like it does
        {
            TryClose();
        }
    }
}
