using System.Windows;
using Caliburn.Micro;
using SpotifyInterface_WPF.ViewModels;

namespace SpotifyInterface_WPF
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
