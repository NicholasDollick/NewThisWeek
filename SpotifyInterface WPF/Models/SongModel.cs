using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace SpotifyInterface_WPF.Models
{
    public class SongModel
    {
        public string SongTitle { get; set; }

        public string SongArtist { get; set; }

        public string SongURI { get; set; }

    }
}
