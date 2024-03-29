# EightTube
EightTube is an old project of mine back in the summer of '23 to get YouTube working on Windows Phone.
This project's growth accelerated, mainly because of my iPhone SE's broken state, and I was making my Nokia Lumia 635 my daily driver.
The app was based on the InvidiousAPI, and used the WinRT Phone with C# in Visual Studio 2015.

Unfortunately, this project's source code, compilation files, and builds have all been lost to a corrupted partition. This project meant a lot to me, as it was a project I was extremely fond about, considering the phone I used for development was my grandfather's Windows Phone.

Since the Windows Phone still works, I made this repository to post images of what the app looked like. To this day, the app still functions, and functions pretty well. All the images were taken on 29/03/2024.

## Homepage
The homepage consisted of 3 items: Popular videos, trending videos, and subscribed videos.
### Popular
![wp_ss_20240329_0001](https://github.com/atomtables/EightTube/assets/76572470/c8695199-5316-4a02-a558-74508f0e77a6)
### Trending
![wp_ss_20240329_0002](https://github.com/atomtables/EightTube/assets/76572470/4bb7057f-b005-4771-a779-98210698b0be)
### Subscribed
![wp_ss_20240329_0006](https://github.com/atomtables/EightTube/assets/76572470/3e3d80d6-4b1e-4490-878a-4e83b269e1fa)
You can subscribe to any channel from that channel's page. "show" moved the user to the channel page.

## Settings
The settings page was pretty barren, and only stored the InvidiousAPI link. On change, the settings page would check if the URL redirected to a genuine InvidiousAPI instance, and show the version of Invidious it was using.
![wp_ss_20240329_0005](https://github.com/atomtables/EightTube/assets/76572470/6b910f71-c635-45eb-9d43-e7a9a0239c9a)

## Search
The search page would use the same search menu that Invidious used. Search can be accessed from the homepage, and shows up a dialog box. Users can go to the next page of results, and filter by videos or channels.
### Search Dialog
![wp_ss_20240329_0007](https://github.com/atomtables/EightTube/assets/76572470/38518041-5379-40b9-948a-470ddda1a7c5)
### Search Results
![wp_ss_20240329_0008](https://github.com/atomtables/EightTube/assets/76572470/689bf584-bbc1-4d83-b46f-7408e4989724)

## Channel Page
The channel page shows basic information about a channel. It shows the videos published, the shorts published, the related channels, and the about section.
Due to some bugs with the InvidiousAPI, shorts would sometimes show up with a different schema than what it usually is. This bug is either caught, or it crashes the app after being caught.
### Channel Videos
![wp_ss_20240329_0011](https://github.com/atomtables/EightTube/assets/76572470/56e31d43-dc22-4ba3-9d11-fdb2855cf36e)
### Channel About
![wp_ss_20240329_0010](https://github.com/atomtables/EightTube/assets/76572470/28ed3b00-a786-440f-bf7f-f101af09854e)

## Video Page
This page is arguably the most important page of the app. It shows information about the video, options to download the video, audio, combo, or stream it. It also shows comments and recommended videos. It shows views, likes, ratio of likes to views, published date, and channel that published the video.
### Video Info
![wp_ss_20240329_0014](https://github.com/atomtables/EightTube/assets/76572470/e563a9e6-97dd-45df-a925-890e149a453e)
### Video Description (3 lines of it)
![wp_ss_20240329_0012](https://github.com/atomtables/EightTube/assets/76572470/ed740f7a-adcc-441c-b53e-5fa18a67226c)
### Video Recommended Videos
![wp_ss_20240329_0013](https://github.com/atomtables/EightTube/assets/76572470/758fa901-76f2-4aaf-bb88-f33e75475c74)
### Video Download Pages
![wp_ss_20240329_0015](https://github.com/atomtables/EightTube/assets/76572470/a0771640-2d5e-4a71-b70c-c4aadfc6db3f)
![wp_ss_20240329_0016](https://github.com/atomtables/EightTube/assets/76572470/025862d2-2758-436d-bca6-2e8af66d5caa)
![wp_ss_20240329_0017](https://github.com/atomtables/EightTube/assets/76572470/40e5e2fe-622a-45ab-863a-8053a128c117)
### Video Watching
The Video streaming quality page looked similar to the video+audio download page.
![wp_ss_20240329_0018](https://github.com/atomtables/EightTube/assets/76572470/d8dc9813-9cc5-49e5-a736-55c84736e782)
(bad quality was from the video, not the app)

All pictures are available on the repo.

If anyone knows how to dump a Windows Phone app onto a computer, and decompile it, please tell me 🙏 
