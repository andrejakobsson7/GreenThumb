using GreenThumb.Models;
using System.Windows;

namespace GreenThumb
{
    /// <summary>
    /// Interaction logic for ManagePlantWindow.xaml
    /// </summary>
    public partial class ManagePlantWindow : Window
    {
        public ManagePlantWindow()
        {
            InitializeComponent();
        }
        public ManagePlantWindow(PlantModel plantToDisplay)
        {
            InitializeComponent();
        }
    }
}
