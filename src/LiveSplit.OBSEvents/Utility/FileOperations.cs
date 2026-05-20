using System.IO;

namespace LiveSplit.OBSEvents.Utility
{
    internal class FileOperations
    {
        public static string StripInvalidFilenameChars(string name)
        {
            // method for stripping invalid chars found here: https://stackoverflow.com/a/23182807
            return string.Join("_", name.Split(Path.GetInvalidFileNameChars()));
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
