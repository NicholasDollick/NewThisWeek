//using SpotifyAPI.Web;
//using SpotifyAPI.Web.Models;
//using SpotifyAPI.Web.Auth;
//using SpotifyAPI.Web.Enums;
//using System;
//using System.Net;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.Windows;
//using System.Windows.Input;
//using System.Windows.Media.Imaging;
//using System.IO;
//using Microsoft.Win32;
//using System.Collections.ObjectModel;
//using System.Threading;
//using System.ComponentModel;
//using System.Windows.Navigation;

//namespace SpotifyInterface_WPF
//{
//    /// <summary>
//    /// Interaction logic for MainWindow.xaml
//    /// </summary>
//    public partial class MainWindowCopy : NavigationWindow
//    {
//        private SpotifyWebAPI spotify;
//        private PrivateProfile profile;
//        private OpenFileDialog ofd = new OpenFileDialog();
//        private string filePath = "";
//        private ObservableCollection<Song> songs = new ObservableCollection<Song>();
//        private double percentDone = 0;
//        private double counter = 0;
//        private SynchronizationContext mainThread;
//        private Thread backgroundThread;

//        public MainWindowCopy()
//        {
//            //InitializeComponent();
//            //DataContext = this.GetSong();
//            //runButton.IsEnabled = false;
//            //Status.Text = "Status: -";
//            //if (mainThread == null)
//            //    mainThread = new SynchronizationContext();
//            //mainThread = SynchronizationContext.Current;
//        }

//        //private async void InitialSetup()
//        //{
//        //    if (!Dispatcher.CheckAccess())
//        //    {
//        //        this.Dispatcher.Invoke(() => InitialSetup());
//        //        return;
//        //    }

//        //    authButton.IsEnabled = false;
//        //    profile = await spotify.GetPrivateProfileAsync();
//        //    userName.Text = profile.DisplayName;
//        //    userCountry.Text = profile.Country;
//        //    userEmail.Text = profile.Email;
//        //    accountType.Text = profile.Product;

//        //    if (profile.Images != null && profile.Images.Count > 0)
//        //    {
//        //        using (WebClient wc = new WebClient())
//        //        {
//        //            byte[] imageBytes = await wc.DownloadDataTaskAsync(new Uri(profile.Images[0].Url));
//        //            userIcon.Source = ByteArrayToImage(imageBytes);
//        //        }
//        //    }
//        //    Mouse.OverrideCursor = Cursors.Arrow;
//        //    runButton.IsEnabled = true;
//        //}

//        private BitmapSource ByteArrayToImage(byte[] input)
//        {
//            var bitmapImage = new BitmapImage();
//            bitmapImage.BeginInit();
//            bitmapImage.StreamSource = new MemoryStream(input);
//            bitmapImage.EndInit();
//            return bitmapImage;
//        }

//        //private async void RunAuthentication()
//        //{
//        //    WebAPIFactory webApiFactory = new WebAPIFactory(
//        //        "http://localhost", 8000, "78c190180d5e4e79baf28a7ad4c04018",
//        //         Scope.UserReadEmail | Scope.PlaylistReadPrivate |
//        //        Scope.PlaylistModifyPublic | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);

//        //    try
//        //    {
//        //        spotify = await webApiFactory.GetWebApi();
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        MessageBox.Show(ex.Message);
//        //    }

//        //    if (spotify == null)
//        //        return;

//        //    InitialSetup();
//        //}

//        private void authButton_Click(object sender, RoutedEventArgs e)
//        {
//            Mouse.OverrideCursor = Cursors.Wait;
//            //Task.Run(() => RunAuthentication());
//        }

//        private void openFileButton_Click(object sender, RoutedEventArgs e)
//        {
//            ofd.Filter = "Text Files|*.txt";
//            var result =  ofd.ShowDialog();
//            if (result.HasValue && result.Value)
//            {
//                fileNameTextBox.Text = ofd.SafeFileName;
//                filePath = ofd.FileName;
//            }
//        }

//        private void runButton_Click(object sender, RoutedEventArgs e)
//        {
//            List<string> tempList = new List<string>();
//            if (playListBox.Items.Count > 0)
//            {
//                percentDone = 0;
//                songs.Clear();
//            }

//            if (fromFile.IsChecked == true)
//            {
//                if (fileNameTextBox.Text == "                       ")
//                    MessageBox.Show("Please Select Valid File");
//                else
//                {
//                    tempList = ReadIn(filePath);
//                    counter = (100 / (double)tempList.Count);
//                    backgroundThread = new Thread(() => CreatePlaylist(tempList));
//                    backgroundThread.Start();
//                }
//            }

//            else if (fromURL.IsChecked == true)
//            {
//                if (siteUrlTextBox.Text == "")
//                    MessageBox.Show("Please Enter Valid URL");
//                else
//                {
//                    tempList = FromWeb.GetData(siteUrlTextBox.Text);
//                    counter = (100 / (double)tempList.Count);
//                    backgroundThread = new Thread(() => CreatePlaylist(tempList));
//                    backgroundThread.Start();
                    
//                }
//            }

//            else
//                MessageBox.Show("Select Date Type");

//        }

//        private List<string> ReadIn(string fileName)
//        {
//            List<string> tracks = new List<string>();
//            Status.Text = "Status: Reading";
//            foreach (string line in File.ReadAllLines(fileName))
//            {
//                if (line != "")
//                {
//                    tracks.Add(CleanAndFormat(line));
//                }
//            }

//            return tracks;
//        }

//        public static string CleanAndFormat(string track)
//        {
//            string[] terms = { "feat.", "ft.", "&" };

//            foreach (string term in terms)
//            {
//                if (track.Contains(term))
//                {
//                    int start = track.IndexOf(term);
//                    int end = track.IndexOf("-");

//                    if (start > end) //this loop would be if term comes after the - in line of text (ie: Artist - Title feat. Artist)
//                        track = track.Substring(0, start);
//                    else
//                        track = track.Substring(0, start) + (track.Substring(end, (track.Length - end)));
//                }
//            }

//            return track;
//        }

//        private ObservableCollection<Song> GetSong()
//        {
//            return songs;
//        }

//        private void CreatePlaylist(List<string> tracks)
//        {
//            FullPlaylist newReleases = spotify.CreatePlaylist(profile.Id, DateTime.Now.ToString("MM/dd") + " Releases");
//            SearchItem song = new SearchItem();
//            ErrorResponse response = new ErrorResponse();

//            if (newReleases.HasError()) //This might need more graceful integration
//                Console.WriteLine(newReleases.Error.Message);

//            // Passes cursor update to main thread
//            mainThread.Post((object state) => { Mouse.OverrideCursor = Cursors.Wait; Status.Text = "Status: Searching"; }, null);
           

//            foreach (string target in tracks)
//            { 
//                if (target.Contains("EP") || target.Contains("Album") || target.Contains("Remixes"))
//                {
//                    song = spotify.SearchItems(target, SearchType.Album);
//                    if (song.Albums.Total > 0)
//                    {
//                        FullAlbum album = spotify.GetAlbum(song.Albums.Items[0].Id);
//                        for (int i = 0; i < album.Tracks.Total; i++)
//                        {
//                            response = spotify.AddPlaylistTrack(profile.Id, newReleases.Id, album.Tracks.Items[i].Uri);

//                            // Passes listbox ui update to main thread
//                            mainThread.Send((object state) => {
//                                songs.Add(new Song() { SongTitle = album.Tracks.Items[i].Name });
//                            }, null);
//                        }
//                    }
//                }
//                else
//                {
//                    song = spotify.SearchItems(target, SearchType.Track);
//                    if (song.Tracks.Items.Count > 0)
//                    {
//                        response = spotify.AddPlaylistTrack(profile.Id, newReleases.Id, song.Tracks.Items[0].Uri);
                       
//                        // Passes listbox ui update to main thread
//                        mainThread.Send((object state) => {
//                            songs.Add(new Song() { SongTitle = song.Tracks.Items[0].Name });
//                        }, null);   
//                    }

//                    if (response.HasError()) //This might need more graceful integration
//                        Console.WriteLine(response.Error.Message);
//                }

//                // pass percent complete updates to main thread
//                mainThread.Send((object state) => {
//                    percentDone += counter;
//                    progressBar.Value = percentDone;
//                    Amount.Text = percentDone.ToString("0.") + "%";
//                }, null);
//            }

//            // Passes cursor update to main thread
//            mainThread.Post((object state) => { Mouse.OverrideCursor = Cursors.Arrow; Status.Text = "Status: Complete"; }, null);
            
//            MessageBox.Show("Playlist Created!");
//            if (response.HasError()) //This might need more graceful integration
//                Console.WriteLine(response.Error.Message);
            
//        }

//        //private void closeButton_Click(object sender, RoutedEventArgs e)
//        //{
//        //    this.Close();
//        //}
//    }
//}
