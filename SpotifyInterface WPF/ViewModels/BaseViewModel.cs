using SpotifyInterface_WPF.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SpotifyInterface_WPF.ViewModels {
    public class BaseViewModel : INotifyPropertyChanged, IDisposable {
        // Global properties could also sit here

        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void EventHandler(object sender, ChangePageEventArgs p);
        public event EventHandler ChangePage;

        public BaseViewModel() {
            ChangePage = null;
        }

        protected void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        protected bool SetField<T>(ref T field, T value, string propertyName) {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnChangePage(ChangePageEventArgs e) {
            EventHandler handler = ChangePage;
            if (handler != null) {
                handler.Invoke(this, e);
            }
        }


        public virtual void Dispose() { }


        #region ChangePage functions
        public void ChangePageToIntro(object parameter) {
            //ChangePageEventArgs args = new ChangePageEventArgs();
            //args.NextPage = typeof(IntroPage);
            //args.StringProperties = new string[] { this.ActiveReport };
            //OnChangePage(args);
        }
        #endregion
    }
}