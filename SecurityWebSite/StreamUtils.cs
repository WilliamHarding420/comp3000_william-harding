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
            process.StartInfo.Arguments = $"-i rtsp://{camera.IP}:{camera.Port}/{camera.StreamURL} " +
                                          $"-f rtsp -rtsp_transport tcp rtsp://admin:admin@mediamtx:8554/{camera.PublishURL}";

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
            process.StartInfo.Arguments = $"-y -rtsp_transport tcp -i rtsp://mediamtx:8554/mystream -update true -vframes 1 /app/Thumbnails/{camera.Name}.png";

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            return Task.CompletedTask;

        }

    }
}
