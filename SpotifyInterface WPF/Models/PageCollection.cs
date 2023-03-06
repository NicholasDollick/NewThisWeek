using SpotifyInterface_WPF.Views;

namespace SpotifyInterface_WPF.Models {
    public class PageCollection {
        private LoginView loginView;

        public PageCollection() { }

        public LoginView LoginView { get => loginView; set => loginView = value; }
    }
}
