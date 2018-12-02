using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using SpotifyInterface_WPF.Models;

namespace SpotifyInterface_WPF.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private string _firstName;
        private BindableCollection<SongModel> _songs = new BindableCollection<SongModel>();
        private SongModel _selectedSong;

        public ShellViewModel()
        {
            Songs.Add(new SongModel { SongTitle = "test" });
            Songs.Add(new SongModel { SongTitle = "Will We Remain?" });
            Songs.Add(new SongModel { SongTitle = "FireFlies" });
            Songs.Add(new SongModel { SongTitle = "Badam" });
            Songs.Add(new SongModel { SongTitle = "Still There's YOu" });
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);
            }
        }

        public BindableCollection<SongModel> Songs
        {
            get { return _songs; }
            set { _songs = value;  }
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
    }
}
