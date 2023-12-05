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

        public PlantWindow()
        {
            InitializeComponent();
            WelcomeUser(UserManager.SignedInUser!);
            LoadLogo();
            GetAllPlantsAsync();
        }

        public void LoadLogo()
        {
            imgLogo.Source = ImageManager.GetLogo(imgLogo.Source);
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            SignOutAndRedirectToLoginPage();
        }

        async private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchWord = txtSearch.Text;
            var filteredList = await GetPlantsByName(searchWord);
            DisplayAllPlants(filteredList);
        }

        private void SignOutAndRedirectToLoginPage()
        {
            UserManager.SignedInUser = null;
            MainWindow mw = new();
            mw.Show();

            Close();
        }

        async private void GetAllPlantsAsync()
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                var plantList = await uow.PlantRepo.GetPlantsWithIncludedDataAsync();
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
                    lstPlants.Items.Remove(selectedItem);
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
            RedirectToManagePlantWindowToAddNewPlant();
        }

        private void RedirectToManagePlantWindowToAddNewPlant()
        {
            ManagePlantWindow mpw = new();
            mpw.Show();

            Close();
        }

        private void RedirectToManagePlantWindowToSeeDetails(PlantModel selectedPlant)
        {
            ManagePlantWindow mpw = new(selectedPlant);
            mpw.Show();

            Close();
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            bool isValidPlant = ValidateItemHasBeenSelected(lstPlants.SelectedItem);
            if (isValidPlant)
            {
                ListBoxItem selectedItem = (ListBoxItem)lstPlants.SelectedItem;
                PlantModel selectedPlant = (PlantModel)selectedItem.Tag;
                RedirectToManagePlantWindowToSeeDetails(selectedPlant);
            }
        }

        private void btnGoToMyGarden_Click(object sender, RoutedEventArgs e)
        {
            RedirectToMyGardenWindow();
        }
        private void RedirectToMyGardenWindow()
        {
            MyGardenWindow mgw = new();
            mgw.Show();
            Close();
        }
    }
}
