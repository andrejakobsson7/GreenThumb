using GreenThumb.Database;
using GreenThumb.Database.Repositories;
using GreenThumb.Managers;
using GreenThumb.Models;
using System.Windows;
using System.Windows.Controls;

namespace GreenThumb
{
    /// <summary>
    /// Interaction logic for PlantWindow.xaml
    /// </summary>
    public partial class PlantWindow : Window
    {
        //Field variable so we don't have to communicate with the database all the time, like when searching..
        List<PlantModel> _plantList = new();

        public PlantWindow(UserModel signedInUser)
        {
            InitializeComponent();
            LoadLogo();
            GetAllPlantsAsync();
            DisplayAllPlants();
        }
        public void LoadLogo()
        {
            imgLogo.Source = ImageManager.GetLogo(imgLogo.Source);
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            RedirectToLoginPage();
        }

        private void RedirectToLoginPage()
        {
            MainWindow mw = new();
            mw.Show();

            Close();
        }

        async private void GetAllPlantsAsync()
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                _plantList = await uow.PlantRepo.GetAllPlantsAsync();
            }
        }

        private void DisplayAllPlants()
        {
            foreach (var plant in _plantList)
            {
                ListBoxItem item = new();
                item.Tag = plant;
                item.Content = plant.ToString();
                lstPlants.Items.Add(item);
            }
        }

    }
}
