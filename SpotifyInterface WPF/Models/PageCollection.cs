using SpotifyInterface_WPF.Views;

namespace SpotifyInterface_WPF.Models {
    public class PageCollection {
        private LoginView loginView;
        private MainAppView mainAppView;

        public PageCollection() { }

        public LoginView LoginView { get => loginView; set => loginView = value; }
        public MainAppView MainAppView { get => mainAppView; set => mainAppView = value; }
    }
}
