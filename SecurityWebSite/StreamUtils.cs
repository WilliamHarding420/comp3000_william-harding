using SecurityWebSite.DatabaseModels;
using System.Diagnostics;

namespace SecurityWebSite
{
    public class StreamUtils
    {

        public static Task PublishCamera(Camera camera)
        {

            Process process = new Process();

            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.Arguments = $"-re -f rtsp -rtsp_transport tcp -i rtsp://{camera.IP}:{camera.Port}/{camera.StreamURL} -c:a copy -c:v libx264 " +
                                          $"-f rtsp -rtsp_transport tcp rtsp://camera:LET_ME_IN@mediamtx:8554/{camera.PublishURL}";

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            return Task.CompletedTask;

        }

        public static Task ReadThumbnailFromStream(Camera camera)
        {

            Process process = new Process();

            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.Arguments = $"-y -rtsp_transport tcp -i rtsp://mediamtx:8554/{camera.PublishURL} -update true -vframes 1 /app/Thumbnails/{camera.Name}.png";

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            return Task.CompletedTask;

        }

    }
}
