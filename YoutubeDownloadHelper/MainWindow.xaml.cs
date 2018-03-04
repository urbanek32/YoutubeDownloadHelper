using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace YoutubeDownloadHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ProcessHelper ProcessHelper { get; set; }
        public ObservableCollection<FileData> Files { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ProcessHelper = new ProcessHelper(OutputDataReceived, ErrorDataReceived);
            Files = new ObservableCollection<FileData>();
            this.DataContext = Files;
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log(e.Data);
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log(e.Data);
        }

        private void Log(string text)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                LogTextBlock.Text += Environment.NewLine + text;
            }));
        }

        private async void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            var youtubeUrl = FileUrlTextBox.Text;
            if (string.IsNullOrWhiteSpace(youtubeUrl))
            {
                MessageBox.Show(this, "Wrong URL", null, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var fileToAdd = new FileData
            {
                FileUrl = youtubeUrl,
                FileStatus = FileStatus.ResolvingTitle
            };

            Files.Add(fileToAdd);

            await ProcessHelper.GetFileTitle(fileToAdd.FileUrl)
                .ContinueWith(t =>
                {
                    var title = t.Result;
                    fileToAdd.Filename = title;
                })
                .ContinueWith(task =>
                {
                    fileToAdd.FileStatus = FileStatus.Downloading;
                    ProcessHelper.DownloadFile(fileToAdd);
                });
        }
    }
}
