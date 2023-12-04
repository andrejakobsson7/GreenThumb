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

        public PlantWindow(UserModel signedInUser)
        {
            InitializeComponent();
            WelcomeUser(signedInUser);
            LoadLogo();
            GetAllPlantsAsync();

        }

        public void LoadLogo()
        {
            imgLogo.Source = ImageManager.GetLogo(imgLogo.Source);
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            RedirectToLoginPage();
        }

        async private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchWord = txtSearch.Text;
            var filteredList = await GetPlantsByName(searchWord);
            DisplayAllPlants(filteredList);
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
                var plantList = await uow.PlantRepo.GetAllPlantsAsync();
                DisplayAllPlants(plantList);
            }
        }

        private void DisplayAllPlants(List<PlantModel> listToDisplay)
        {
            lstPlants.Items.Clear();
            foreach (var plant in listToDisplay)
            {
                ListBoxItem item = new();
                item.Tag = plant;
                item.Content = plant.ToString();
                lstPlants.Items.Add(item);
            }
        }
        async private Task<List<PlantModel>> GetPlantsByName(string plantName)
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                return await uow.PlantRepo.GetPlantsByNameAsync(plantName);
            }
        }
        private void WelcomeUser(UserModel signedInUser)
        {
            lblWelcomeUser.Content = $"Welcome {signedInUser.Username}";
        }

        async private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            bool isValidItem = ValidateItemHasBeenSelected(lstPlants.SelectedItem);
            if (isValidItem)
            {
                ListBoxItem selectedItem = (ListBoxItem)lstPlants.SelectedItem;
                PlantModel selectedPlant = (PlantModel)selectedItem.Tag;
                MessageBoxResult answer = MessageBox.Show($"Please confirm that you want to remove {selectedPlant.Name}.", "Confirm removal", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (answer == MessageBoxResult.OK)
                {
                    await RemovePlantAsync(selectedPlant.PlantId);
                    GetAllPlantsAsync();
                }
            }
        }

        private bool ValidateItemHasBeenSelected(object item)
        {
            if (item == null)
            {
                MessageBox.Show("No plant has been selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        async private Task RemovePlantAsync(int plantId)
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                await uow.PlantRepo.RemovePlant(plantId);
                await uow.CompleteAsync();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
