using System.IO;
using System.Windows.Media.Imaging;

namespace GreenThumb.Managers
{
    public static class ImageManager
    {
        public static BitmapImage? GetPlantImage(string filePath)
        {
            if (filePath != "" && filePath != null)
            {
                BitmapImage newBmI = new();
                newBmI.BeginInit();
                newBmI.DecodePixelWidth = 220;
                newBmI.UriSource = new Uri($@"{filePath}");
                newBmI.EndInit();
                return newBmI;
            }
            return null;
        }
        public static string GetImageUrl()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            Guid newGuid = CreateGuid();
            FileStream fs = File.Create($"{currentDirectory}\\{newGuid}.jpg");
            fs.Dispose();
            return fs.Name;
        }

        public static void AddImageToFolder(string fileToCopy, string fileDestination)
        {
            File.Copy(fileToCopy, fileDestination, true);
        }

        public static Guid CreateGuid()
        {
            return Guid.NewGuid();
        }





    }
}
