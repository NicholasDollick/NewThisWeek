# NewThisWeek
_**tl;dr:** Takes an input either as text file or url, and creates a spotify playlist of all the newest releases of that week via the spotify web API._

As a music lover, a large chunk of time gets spent Thursday nights into Friday mornings creating a playlist of all the newest releases. 
![alt text](https://i.imgur.com/IJarnt6.jpg)

A subreddit I'm active on crowdsources all of the week's new tracks into one convenient post. This master post and the amount of time wasted curating the playlist was the inspiration for this applet. 



## Overview
![alt text](https://i.imgur.com/kgGrNZQ.jpg)
Pictured above are both the Winform and WPF versions of the applet. In both instances, users must first authenticate their sessions. This will prompt for users to accept the requested spotify permissions and load their profile into the UI.

![alt text](https://i.imgur.com/E0M2xdh.jpg)
Once loaded, users can select a .txt containing a list of tracks they want added to the playlist and load it into the applet. During runtime, the file will be read, the input will be cleaned for better search efficiency, and tracks will be added. 
Currently, the parsing methods search for tags similar to the ones used in the reddit posts:
```
Andrew Rayel - In The Dark

Chris Lake & Walker & Royce - Close Your Eyes EP

Louis The Child - Better Not (Remixes)

Chicane - The Place You Can't Remember, The Place You Can't Forget (Album)
```
With the inclusion of "EP", "Remixes", and "Album" signifying to search for album listings during the spotify search, instead of grabbing the first listed track. 

Data from webpage takes provided URL and extracts data from it. At this time, this service only works for reddit thread urls. This data is parsed and passed to main thread to be turned into a playlist.

The time monitor option is currently hardcoded to PST, an option to set your own time is coming soon. It lets the user set the applet to wait until midnight EST (the time spotify updates tracks for the US) to execute and create the playlist.

When completed the applet will display all the tracks added in a list, and a playlist will have been added to the users spotify account. By default, the list is titled as the date it was executed, in MM/dd format, followed by "Releases".
![alt text](https://i.imgur.com/4xLtpMN.jpg)


packages used
```
HtmlAgilityPack
Reddit API
SpotifyAPI-NET
```
