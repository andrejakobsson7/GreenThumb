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
            GetAllPlantsAsync();
        }

        private void WelcomeUser(UserModel signedInUser)
        {
            lblWelcomeUser.Content = $"Welcome {signedInUser.Username}";
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            SignOutAndRedirectToLoginPage();
        }
        private void SignOutAndRedirectToLoginPage()
        {
            UserManager.SignedInUser = null;
            MainWindow mw = new();
            mw.Show();

            Close();
        }

        async private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchWord = txtSearch.Text;
            var filteredList = await SearchPlantsByNameAsync(searchWord);
            DisplayAllPlants(filteredList);

        }

        async private void GetAllPlantsAsync()
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                var plantList = await uow.PlantRepo.GetAllPlantsWithIncludedDataAsync();
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
        async private Task<List<PlantModel>> SearchPlantsByNameAsync(string searchWord)
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                return await uow.PlantRepo.GetPlantsByNameAsync(searchWord);
            }
        }
        async private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            bool isValidItem = ValidateItemHasBeenSelected(lstPlants.SelectedItem);
            if (isValidItem)
            {
                ListBoxItem selectedItem = (ListBoxItem)lstPlants.SelectedItem;
                PlantModel selectedPlant = (PlantModel)selectedItem.Tag;
                MessageBoxResult answer = MessageBox.Show($"Please confirm that you want to remove {selectedPlant.Name}. It will be removed from every member's garden!", "Confirm removal", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (answer == MessageBoxResult.OK)
                {
                    await RemovePlantAsync(selectedPlant.PlantId, selectedItem);
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
        async private Task RemovePlantAsync(int plantId, ListBoxItem selectedItem)
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                bool isRemoved = await uow.PlantRepo.RemovePlantAsync(plantId);
                if (isRemoved)
                {
                    await uow.CompleteAsync();
                    MessageBox.Show("Plant was succesfully removed!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    lstPlants.Items.Remove(selectedItem);
                }
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
