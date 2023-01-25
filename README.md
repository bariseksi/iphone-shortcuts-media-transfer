# iphone-shortcuts-media-transfer
Directly transfer your photo, video or any media to any PC wirelessly over the lan via IPhone's built-in Shortcuts application with Http Post request without any necessity of 3rd party drive/web/internet services. 

## How To Make It Work
Your IPhone and the PC you want to transfer, both must be in the same network.
First you need to compile the "ShortcutsListener" C# .net project and run the application(server) in windows or linux.
(compiled version exists for windows users in ShortcutsListener/ShortcutsListener/bin/Release folder).
You need to create a "shortcut" from IPhone's built-in Shortcuts application by tapping + sign.
You can think "shortcut" as a kind of script runs in IPhone to let us send media files to our server.

<p align="left">
  <img src="images/img_001.jpeg" width="350">
</p>



then actions are seen below needs to be added.

- Add the "URL" action and set the IP address of your computer and the port (default port is set to 2560 in the server app).
If your pc has a IPAddress of 192.168.1.10 then the "URL" action should be http://192.168.1.10:2560 the number after the colon is the port number.
- Add the "Select photos" action with "Select Multiple" option turned on and "Include" option set to "All". This action opens the photo app of your phone and lets you select photos or videos when you run the completed shortcut.
- Since "Select photos" returns "Photos" variable that is contains more than one media we need to iterate each of them by adding "Repeat with Each" action.
- Inside the "Repeat with Each" action "Get contents of" action needs to be added. This is the action actually does the job.
<p align="left">
  <img src="images/img_002.png" width="350">
</p>

"Get contents of" action is the action that makes the HTTP request.
Configure the contents of the "Get contents of" action like below.
- Set the HTTP request method as POST.
- "filename" header must be added to the HTTP request and "Repeated Item" needs to be pointed in order to get filename.
- You need to set the URL of the "Get contents of" action to the url you added previously.
<p align="left">
  <img src="images/img_003.png" width="350">
</p>

Now you can start the server and then run the shortcut.
All selected photos and videos will be transfered to PC.
You may need to download required codecs to your computer to view your files.

You can do other shortcuts that can send other file types from IPhone's "Files" application. Its up to your imagination.
- Note: Transfer stops as soon as iPhone screen goes off. You need to keep the screen on while you are transfering too many photos or huge videos.
