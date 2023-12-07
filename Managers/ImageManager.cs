using System.IO;
using System.Windows.Media.Imaging;

namespace GreenThumb.Managers
{
    public static class ImageManager
    {
        //Method used to be able to display plants images in the UI.
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

        //Create an empty file with a randomized name in the project folder.
        public static string GetImageUrl()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            Guid newGuid = CreateGuid();
            FileStream fs = File.Create($"{currentDirectory}\\{newGuid}.jpg");
            fs.Dispose();
            return fs.Name;
        }
        //Copy file from user to project folder
        public static void AddImageToFolder(string fileToCopy, string fileDestination)
        {
            File.Copy(fileToCopy, fileDestination, true);
        }

        //Create a new unique name for the file.
        public static Guid CreateGuid()
        {
            return Guid.NewGuid();
        }





    }
}
