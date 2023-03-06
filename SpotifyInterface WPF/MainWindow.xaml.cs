using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Windows;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Navigation;
using System.Windows.Threading;
using SpotifyInterface_WPF.ViewModels;

namespace SpotifyInterface_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitAfterLoad() {
            this.DataContext = new ApplicationViewModel(this);
        }


        private void Application_Loaded(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke(new Action(() => InitAfterLoad()), DispatcherPriority.ContextIdle, null);
        }
    }
}
