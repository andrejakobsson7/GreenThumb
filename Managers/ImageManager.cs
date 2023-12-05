using System.Windows.Media.Imaging;

namespace GreenThumb.Managers
{
    public static class ImageManager
    {
        public static BitmapImage GetLogo(System.Windows.Media.ImageSource imgLogo)
        {
            BitmapImage newBmI = new();
            newBmI.BeginInit();
            newBmI.UriSource = new Uri($@"C:\Users\andre\OneDrive\Dokument\Databas\Övningar\GreenThumb\GreenThumb\Images\green-thumb-logo.png");
            newBmI.EndInit();
            return newBmI;
        }

        public static BitmapImage GetPlantImage(string filePath)
        {
            BitmapImage newBmI = new();
            newBmI.BeginInit();
            newBmI.DecodePixelWidth = 220;
            newBmI.UriSource = new Uri($@"{filePath}");
            newBmI.EndInit();
            return newBmI;
        }

    }
}
