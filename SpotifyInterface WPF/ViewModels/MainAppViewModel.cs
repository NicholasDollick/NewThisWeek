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
using SpotifyAPI.Web.Models;
using System.Net;
using System.Windows.Media.Imaging;
using System.IO;
using System.Threading;

namespace SpotifyInterface_WPF.ViewModels
{
    public class MainAppViewModel : Screen
    { 
        private string _userName = "-";
        private string _userCountry = "-";
        private string _userEmail = "-";
        private string _userAccountType = "-";
        private bool _fromFile = false;
        private bool _fromWeb = false;
        private string _fileName = "";
        private string _filePath = "";
        private double _percentDone = 55;
        private string _amount = "0%";
        private SpotifyWebAPI _spotify;
        private PrivateProfile _profile;
        private static string DefaultImage = "https://i.imgur.com/8IHaKKE.png";
        private BitmapImage icon = new BitmapImage(new Uri(DefaultImage, UriKind.Absolute));
        private BindableCollection<SongModel> _songs = new BindableCollection<SongModel>();
        private SongModel _selectedSong;
        private SynchronizationContext mainThread;
        private readonly Logic Controller = new Logic();
        private readonly SongModel model;
        private BindableCollection<SongModel> test = new BindableCollection<SongModel>();

        public MainAppViewModel()
        {
            if (mainThread == null)
                mainThread = new SynchronizationContext();
            mainThread = SynchronizationContext.Current;
        }

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

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyOfPropertyChange(() => FileName);
            }
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                NotifyOfPropertyChange(() => FilePath);
            }
        }

        public bool FromFile
        {
            get { return _fromFile;  }
            set
            {
                _fromFile = value;
                NotifyOfPropertyChange(() => FromFile);
            }
        }

        public bool FromWeb
        {
            get { return _fromWeb; }
            set
            {
                _fromWeb = value;
                NotifyOfPropertyChange(() => FromWeb);
            }
        }

        public double PercentDone
        {
            get { return _percentDone; }
            set
            {
                _percentDone = value;
                NotifyOfPropertyChange(() => PercentDone);
            }
        }

        public string Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                NotifyOfPropertyChange(() => Amount);
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

            // this entire method might be able to be done in external class.
            // would allow the view to just display whatever got passed back?
            // perhaps even toss all of the spotify api things into a logic class
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

        public BindableCollection<SongModel> Songs
        {
            get { return _songs; }
            set { _songs = value; }
        }

        public SongModel SelectedSong
        {
            get { return _selectedSong; }
            set
            {
                _selectedSong = value;
                NotifyOfPropertyChange(() => SelectedSong);
            }
        }


        public bool CanRunLogic(string userName)
        {
            if (userName.Equals("-"))
                return false;
            else
                return true;
        }

        public async void RunLogic(string userName)
        {
            if(FromFile)
                try
                {
                    await Task.Run(() =>
                   Controller.CreatePlaylist(_songs, _spotify, _profile, null);
                        );
                } catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }


            await Task.Run(() =>
            {
                while (true)
                {
                    if (FromWeb)
                        Controller.test(_songs);
                    
                    Thread.Sleep(5000);
                    
                }
            });

        }
    }
}
