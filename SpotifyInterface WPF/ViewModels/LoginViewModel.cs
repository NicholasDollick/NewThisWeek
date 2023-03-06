using SpotifyInterface_WPF.Helpers;
using System;
using System.Windows.Input;

namespace SpotifyInterface_WPF.ViewModels
{
    class LoginViewModel : BaseViewModel {
        private ICommand _loginCommand;
        private bool _canExecute;

        
        public LoginViewModel() {
            LoginCommand = new RelayCommand(AttemptLogin, param => _canExecute);
        }

        internal void AttemptLogin(object _) {
            throw new NotImplementedException();
        }


        public ICommand LoginCommand
        {
            get
            {
                return this._loginCommand;
            }

            set
            {
                this._loginCommand = value;
            }
        }
    }
}