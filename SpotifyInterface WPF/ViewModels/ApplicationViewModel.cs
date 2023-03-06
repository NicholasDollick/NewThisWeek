using SpotifyInterface_WPF.Helpers;
using SpotifyInterface_WPF.Models;
using SpotifyInterface_WPF.Views;
using System;
using System.Windows;

namespace SpotifyInterface_WPF.ViewModels {

    // I'm still not a huge fan of how I handle page changes here
    public class ApplicationViewModel : BaseViewModel {
        private MainWindow mainNavWindow;
        private PageCollection PageCollection;
        private Type currentViewModel;
        
        public ApplicationViewModel(MainWindow mainWindow) {
            this.mainNavWindow = mainWindow;

            PageCollection = new PageCollection();
            if(PageCollection.LoginView == null) {
                PageCollection.LoginView = mainNavWindow.Content as LoginView;
            }
            currentViewModel = typeof(LoginView);
            //Set the event handler "ChangePage" on the current ViewModel to trigger a navigation to another view via "PageChanged" method
            ((LoginViewModel)PageCollection.LoginView.DataContext).ChangePage += PageChanged;

            Application.Current.Dispatcher.Invoke(
                () => {
                    PageCollection.LoginView.Visibility = Visibility.Visible;
                });
        }


        /// <summary>
        /// Event Handler method for navigating to next page
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">data needed from the event triggered</param>
        private void PageChanged(object sender, ChangePageEventArgs e) { // would having this public remove the need to constantly define functions in the viewmodels?
            if (e.NextPage == typeof(LoginView)) {
                if (PageCollection.LoginView == null) {
                    PageCollection.LoginView = new LoginView();
                    (PageCollection.LoginView.DataContext as LoginViewModel).ChangePage += PageChanged;
                }
                //this.MainNavWindow.Navigate(PageCollection.LoginView);
                currentViewModel = e.NextPage;
            }
        }


        public MainWindow MainNavWindow
        {
            get
            {
                return this.mainNavWindow;
            }
            set
            {
                this.mainNavWindow = value;
            }
        }
    }
}