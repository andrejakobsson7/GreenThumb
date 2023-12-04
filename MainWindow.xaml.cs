using GreenThumb.Managers;
using System.Windows;

namespace GreenThumb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadLogo();
        }

        private void LoadLogo()
        {
            imgLogo.Source = ImageManager.GetLogo();
        }
    }
}