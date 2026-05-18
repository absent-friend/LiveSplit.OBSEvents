using System;
using System.IO;

namespace LiveSplit.GoldGrabber
{
    internal class FileUtils
    {
        public static string StripInvalidFilenameChars(string name)
        {
            // method for stripping invalid chars found here: https://stackoverflow.com/a/23182807
            return string.Join("_", name.Split(Path.GetInvalidFileNameChars()));
        }

        public static string FormatTimeSpanForFilename(TimeSpan timeSpan)
        {
            string format;
            if (timeSpan.Hours > 0)
            {
                format = "h'h'mm'm'ss's'fff'ms'";
            }
            else if (timeSpan.Minutes > 0)
            {
                format = "m'm'ss's'fff'ms'";
            }
            else
            {
                format = "s's'fff'ms'";
            }
            return timeSpan.ToString(format);
        }

        public static void Rename(string path, string newName)
        {
            string directory = Path.GetDirectoryName(path);
            string extension = Path.GetExtension(path);
            string newPath = Path.Combine(directory, $"{newName}{extension}");
            File.Move(path, newPath);
        }
    }
}
