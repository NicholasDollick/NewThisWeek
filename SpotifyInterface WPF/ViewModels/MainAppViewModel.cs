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
        private string _amount = "55%";
        private static readonly string DefaultImage = "https://i.imgur.com/8IHaKKE.png";
        private BitmapImage icon = new BitmapImage(new Uri(DefaultImage, UriKind.Absolute));
        private BindableCollection<SongModel> _songs = new BindableCollection<SongModel>();
        private readonly Logic Controller = new Logic();

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

        public void AuthSession(string userName)
        {
            InitialSetup();
        }

        private async void InitialSetup()
        {
            await Task.Run(() =>
            {
                Controller.Auth();
            });
            UserName = Controller.GetDisplayName();
            UserCountry = Controller.GetCountry();
            UserEmail = Controller.GetEmail();
            UserAccountType = Controller.GetProduct();
            icon = Controller.GetImage();

            NotifyOfPropertyChange(() => DisplayedImage);
        }

        public BindableCollection<SongModel> Songs
        {
            get { return _songs; }
            set { _songs = value; }
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
            if (FromFile)
            {
                Controller.InitPlaylist();

                // this needs to be in a try catch loop when testing phase complete 
                foreach (var track in Controller.ReadIn("Releases.txt"))
                    await Task.Run(() => {
                        _songs.Add(Controller.test(track));
                        Console.WriteLine(track);
                    });
            }
        }
    }
}
