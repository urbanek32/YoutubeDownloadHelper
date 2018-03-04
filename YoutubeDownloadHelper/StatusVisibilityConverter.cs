using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace YoutubeDownloadHelper
{
    /// <summary>
    /// Visible only when FileStatus == Downloading
    /// Except when Reverse mode is Enabled
    /// </summary>
    public class StatusVisibilityConverter: IValueConverter
    {
        public bool Reverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FileStatus fileStatus)
            {
                if (fileStatus == FileStatus.Downloading)
                {
                    return Reverse ? Visibility.Hidden : Visibility.Visible;
                }
            }
            return Reverse ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Visibility)value;

            if (visibility == Visibility.Visible)
            {
                return !Reverse;
            }
            else
            {
                return Reverse;
            }
        }
    }
}
