# Physical Security Dashboard

## Project Vision
The vision of this project is to provide a host in-premises web GUI that can be used to manage your physical security measure (CCTV, etc.). The goal of the project is to provide an easily extensible web interface where you enable/disable certain parts of it depending on what security measures you have in place.   

By supporting as many common protocol for devices such as CCTV cameras (such as RTSP), I hope to provide a product that will be able to work with as many different devices as possible to allow companies and individuals alike to have one centralized area to manage their security.

### Build Steps

Clone the git repository.   
Make sure docker is running.   

In a command line in the cloned foler: ```docker compose up --build```   
This will error once building the solution is finished due to missing SSL certificates in the newly created cert folder.   
Put the following certificate files in the cert folder:

 - domain.crt
 - domain.rsa
 - websitecert.pfx
   
Once those are in the cert folder, running the compose file again should launch the solution without error.

# Tools / Libraries Used
[MediaMTX](https://github.com/bluenviron/mediamtx) is utilized to provide streaming content for CCTV camera feeds, allowing a number of protocol inputs and outputs.   
[HLS.js](https://github.com/video-dev/hls.js/) is used in order to stream HLS content from the [MediaMTX](https://github.com/bluenviron/mediamtx) server onto a HTML web page.   
[Docker](https://www.docker.com/) used to orchestrate containers for the different parts of the solution, eg. database, website / API, etc.   
[FFMPEG](https://www.ffmpeg.org/) enables the video publishing to [MediaMTX](https://github.com/bluenviron/mediamtx).   
[JQuery](https://jquery.com/) used to simplify HTTP requests between client and server.   
[Bootstrap](https://getbootstrap.com/) heavily assisted in the styling of the website.   
[Entity Framework Core](https://github.com/dotnet/efcore) used for interacting with the database.   

