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

        public ShellViewModel()
        {
            // Songs.Add(new SongModel { SongTitle = "FireFlies" });
            LoadMain();
            //LoadLogin();
        }
        
        public void LoadMain()
        {
            ActivateItem(new MainAppViewModel());
        }
        
        public void LoadLogin()
        {
            ActivateItem(new LoginViewModel());
        } 
    }
}
