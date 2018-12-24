using System.Windows;
using Caliburn.Micro;
using SpotifyInterface_WPF.ViewModels;

namespace SpotifyInterface_WPF
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        protected override void Configure()
        {
            _container.Singleton<IEventAggregator, EventAggregator>();
        }

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
