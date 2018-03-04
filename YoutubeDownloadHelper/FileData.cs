using System;
using System.Diagnostics;

namespace YoutubeDownloadHelper
{
    public class FileData: NotifyPropertyChanged
    {
        private string _filename;
        private FileStatus _fileStatus;
        private string _fileUrl;
        private int _downloadProgress;

        public Process DownloadProcess { get; set; }

        public string Filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                OnPropertyChanged("Filename");
            }
        }

        public string FileUrl
        {
            get { return _fileUrl; }
            set
            {
                _fileUrl = value;
                OnPropertyChanged("FileUrl");
            }
        }

        public FileStatus FileStatus
        {
            get { return _fileStatus; }
            set
            {
                _fileStatus = value;
                OnPropertyChanged("FileStatus");
            }
        }

        public int DownloadProgress
        {
            get { return _downloadProgress; }
            set
            {
                _downloadProgress = value;
                OnPropertyChanged("DownloadProgress");
            }
        }

        public void ProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;
            if (string.IsNullOrWhiteSpace(data))
            {
                return;
            }

            var percentageString = Utils.FindPercentInString(data);
            if (!string.IsNullOrWhiteSpace(percentageString))
            {
                var percentage = Utils.FromPercentageString(percentageString);
                DownloadProgress = Convert.ToInt32(percentage);

                if (DownloadProgress >= 100)
                {
                    FileStatus = FileStatus.Done;
                }
            }
        }
    }
}
