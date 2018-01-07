using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YoutubeDownloadHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            Run_cmd();
        }

        private void Run_cmd()
        {
            var start = new Process
            {
                StartInfo =
                {
                    FileName = "C:\\Users\\Patryk\\Desktop\\youtube-dl.exe",
                    Arguments = "--newline https://www.youtube.com/watch?v=SewpndxZDl0",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                }
            };

            start.OutputDataReceived += Start_OutputDataReceived;
            start.ErrorDataReceived += Start_ErrorDataReceived;

            start.Start();
            start.BeginOutputReadLine();
        }

        private void Start_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Start_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                textBlock1.Text += Environment.NewLine + e.Data;
            }));
        }
    }
}
