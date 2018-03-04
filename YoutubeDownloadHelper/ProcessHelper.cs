using System.Diagnostics;
using System.Threading.Tasks;

namespace YoutubeDownloadHelper
{
    public class ProcessHelper
    {
        private DataReceivedEventHandler OutputDataReceived { get; set; }
        private DataReceivedEventHandler ErrorDataReceived { get; set; }

        public ProcessHelper(DataReceivedEventHandler outputDataReceived, DataReceivedEventHandler errorDataReceived)
        {
            this.OutputDataReceived = outputDataReceived;
            this.ErrorDataReceived = errorDataReceived;
        }

        public async Task<string> GetFileTitle(string fileUrl)
        {
            var processArgs = string.Format("{0} {1}", "--get-title", fileUrl);
            return await StartProcessAsync(processArgs);
        }

        public void DownloadFile(FileData fileData)
        {
            var processArgs = string.Format("{0} {1}", "--newline", fileData.FileUrl);
            fileData.DownloadProcess = StartProcessAsync(processArgs, fileData.ProcessOutputDataReceived);
        }

        private Process StartProcessAsync(string arguments, DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null)
        {
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = "Resources//youtube-dl.exe",
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                }
            };

            proc.OutputDataReceived += outputDataReceived ?? OutputDataReceived;
            proc.ErrorDataReceived += errorDataReceived ?? ErrorDataReceived;

            proc.Start();
            proc.BeginOutputReadLine();

            return proc;
        }

        private async Task<string> StartProcessAsync(string arguments)
        {
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = "Resources//youtube-dl.exe",
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            proc.Start();
            var response = await proc.StandardOutput.ReadToEndAsync();
            var err = await proc.StandardError.ReadToEndAsync();

            if (!string.IsNullOrWhiteSpace(err))
            {
                return err.TrimEnd('\n');
            }

            proc.WaitForExit(3000);
            return response.TrimEnd('\n');
        }
    }
}
