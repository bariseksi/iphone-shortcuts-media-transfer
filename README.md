# iphone-shortcuts-media-transfer
Transfer your photo, video or any media to PC wirelessly via IPhone's Shortcuts application with Http Post request.

## How To Make It Work
Your IPhone and the PC you want to transfer to both must be in the same network.
In order to transfer media from your IPhone you need to create a shortcut from IPhone's Shortcuts application by pressing + sign.

<p align="left">
  <img src="images/img_001.jpeg" width="350">
</p>



then actions are seen below needs to be added.
Set the IP address of your computer and the port to URL action (default port is 2560)
<p align="left">
  <img src="images/img_002.png" width="350">
</p>

configure the contents of the "Get contents of" action like below.
"filename" header must be added and "Repeated Item" needs to be pointed in order to get filename.
and you need to set the URL you added previously.

<p align="left">
  <img src="images/img_003.png" width="350">
</p>

