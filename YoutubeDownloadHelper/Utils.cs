using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace YoutubeDownloadHelper
{
    public static class Utils
    {
        public static string FindPercentInString(string input)
        {
            Debug.WriteLine(input);

            // Find percent like 1.5% or 100%
            var pattern = @"(\d+(\.\d+)?%)";

            var match = Regex.Match(input, pattern);
            if (match.Success)
            {
                Debug.WriteLine("MATCH: " + match.Captures[0].Value);
                return match.Captures[0].Value;
            }
            return string.Empty;
        }

        public static decimal FromPercentageString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return decimal.Zero;
            }

            var pieces = input.Split('%');
            if (pieces.Length > 2 || !string.IsNullOrWhiteSpace(pieces[1]))
            {
                return decimal.Zero;
            }
            return decimal.Parse(pieces[0], NumberStyles.Any, CultureInfo.InvariantCulture);
        }
    }
}
