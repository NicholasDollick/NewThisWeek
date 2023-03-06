using System;

namespace SpotifyInterface_WPF.Helpers {
    public class ChangePageEventArgs {
        // Tracking the page to navigate to
        public Type NextPage { get; set; }

        // This is a property that can hold internal data to be passed onto the page being loaded. Can be used inplace of global variables
        public string[] StringProperties { get; set; }
    }
}