using System.Windows.Media.Imaging;

namespace GreenThumb.Managers
{
    public static class ImageManager
    {
        public static BitmapImage GetLogo()
        {
            BitmapImage newBmI = new();
            newBmI.BeginInit();
            newBmI.UriSource = new Uri($@"C:\Users\andre\OneDrive\Dokument\Databas\Övningar\GreenThumb\GreenThumb\Images\green-thumb-logo.png");
            newBmI.EndInit();
            return newBmI;
        }

    }
}
