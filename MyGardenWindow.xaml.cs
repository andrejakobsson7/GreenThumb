using GreenThumb.Database;
using GreenThumb.Database.Repositories;
using GreenThumb.Managers;
using GreenThumb.Models;
using System.Windows;
using System.Windows.Controls;

namespace GreenThumb
{
    /// <summary>
    /// Interaction logic for MyGardenWindow.xaml
    /// </summary>
    public partial class MyGardenWindow : Window
    {
        public MyGardenWindow()
        {
            InitializeComponent();
            LoadLogo();
            DisplayGardenName();
            GetPersonalGardenPlantsByIdAsync();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            RedirectToPlantWindow();
        }

        private void LoadLogo()
        {
            imgLogo.Source = ImageManager.GetLogo(imgLogo.Source);
        }
        private void RedirectToPlantWindow()
        {
            PlantWindow pw = new();
            pw.Show();
            Close();
        }

        private void DisplayGardenName()
        {
            txtGardenName.Text = UserManager.SignedInUser!.Garden.GardenName;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ActivateEditMode();
        }

        async private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool isValidGardenName = ValidateGardenName(txtGardenName.Text);
            if (isValidGardenName)
            {
                using (AppDbContext context = new())
                {
                    GreenThumbUow uow = new(context);
                    await uow.GardenRepo.UpdateGardenNameAsync(UserManager.SignedInUser!.Garden, txtGardenName.Text);
                    await uow.CompleteAsync();
                    MessageBox.Show("Garden name was successfully updated!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    UserManager.SignedInUser!.Garden.GardenName = txtGardenName.Text;
                    DisplayGardenName();
                    ActivateReadOnlyMode();
                }
            }
        }
        private void ActivateReadOnlyMode()
        {
            txtGardenName.IsReadOnly = true;
            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Collapsed;
        }
        private void ActivateEditMode()
        {
            txtGardenName.IsReadOnly = false;
            btnEdit.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            bool isValidGardenPlant = ValidateItemHasBeenSelected(lstPlants.SelectedItem);
            if (isValidGardenPlant)
            {
                ListBoxItem selectedItem = (ListBoxItem)lstPlants.SelectedItem;
                GardenPlant selectedGardenPlant = (GardenPlant)selectedItem.Tag;
                RedirectToManagePlantWindow(selectedGardenPlant.Plant);
            }
        }

        async private void GetPersonalGardenPlantsByIdAsync()
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                var gardenPlants = await uow.GardenPlantRepo.GetAllGardensByGardenIdWithRelatedDataAsync(UserManager.SignedInUser!.Garden.GardenId);
                DisplayAllPlants(gardenPlants);
            }
        }

        private void DisplayAllPlants(List<GardenPlant> listToDisplay)
        {
            lstPlants.Items.Clear();
            foreach (var gardenPlant in listToDisplay)
            {
                ListBoxItem item = new();
                item.Tag = gardenPlant;
                item.Content = gardenPlant.Plant.ToString();
                lstPlants.Items.Add(item);
            }
        }

        async private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            //Remove from personal garden, not the entire database
            bool isValidGardenPlant = ValidateItemHasBeenSelected(lstPlants.SelectedItem);
            if (isValidGardenPlant)
            {
                ListBoxItem selectedItem = (ListBoxItem)lstPlants.SelectedItem;
                GardenPlant selectedGardenPlant = (GardenPlant)selectedItem.Tag;
                using (AppDbContext context = new())
                {
                    GreenThumbUow uow = new(context);
                    await uow.GardenPlantRepo.RemoveGardenPlant(selectedGardenPlant.GardenId, selectedGardenPlant.PlantId);
                    await uow.CompleteAsync();
                }
                lstPlants.Items.Remove(selectedItem);
            }

        }
        private bool ValidateGardenName(string gardenName)
        {
            if (string.IsNullOrWhiteSpace(gardenName))
            {
                MessageBox.Show("No garden name has been entered!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void RedirectToManagePlantWindow(PlantModel plantToDisplay)
        {
            ManagePlantWindow mpw = new(plantToDisplay);
            mpw.Show();
            Close();

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
    }
}
