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
            DisplayGardenName();
            GetPersonalGardenPlantsByIdAsync();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            RedirectToPlantWindow();
        }

        private void RedirectToPlantWindow()
        {
            PlantWindow pw = new();
            pw.Show();
            Close();
        }

        private void DisplayGardenName()
        {
            txtGardenName.Text = UserManager.SignedInUser!.Garden!.GardenName;
        }

        private void btnEditGardenName_Click(object sender, RoutedEventArgs e)
        {
            ActivateEditMode();
        }

        async private void btnSaveGardenName_Click(object sender, RoutedEventArgs e)
        {
            bool isValidGardenName = ValidateGardenName(txtGardenName.Text);
            if (isValidGardenName)
            {
                using (AppDbContext context = new())
                {
                    GreenThumbUow uow = new(context);
                    await uow.GardenRepo.UpdateGardenNameAsync(UserManager.SignedInUser!.Garden!, txtGardenName.Text);
                    await uow.CompleteAsync();
                    MessageBox.Show("Garden name was successfully updated!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    UserManager.SignedInUser!.Garden!.GardenName = txtGardenName.Text;
                    DisplayGardenName();
                    ActivateReadOnlyMode();
                }
            }
        }
        private void ActivateReadOnlyMode()
        {
            txtGardenName.IsReadOnly = true;
            btnEditGardenName.Visibility = Visibility.Visible;
            btnSaveGardenName.Visibility = Visibility.Collapsed;
        }
        private void ActivateEditMode()
        {
            txtGardenName.IsReadOnly = false;
            btnEditGardenName.Visibility = Visibility.Collapsed;
            btnSaveGardenName.Visibility = Visibility.Visible;
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
                var gardenPlants = await uow.GardenPlantRepo.GetAllGardensByGardenIdWithRelatedDataAsync(UserManager.SignedInUser!.Garden!.GardenId);
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
            bool isValidGardenPlant = ValidateItemHasBeenSelected(lstPlants.SelectedItem);
            if (isValidGardenPlant)
            {
                ListBoxItem selectedItem = (ListBoxItem)lstPlants.SelectedItem;
                GardenPlant selectedGardenPlant = (GardenPlant)selectedItem.Tag;
                await RemoveGardenPlantAsync(selectedGardenPlant.GardenId, selectedGardenPlant.PlantId, selectedItem);
                ResetImage();
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
        private void ResetImage()
        {
            imgPlant.Source = null;
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

        private void lstPlants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstPlants.SelectedItem != null)
            {
                ListBoxItem selectedItem = (ListBoxItem)lstPlants.SelectedItem;
                GardenPlant selectedGardenPlant = (GardenPlant)selectedItem.Tag;
                imgPlant.Source = ImageManager.GetPlantImage($"{selectedGardenPlant.Plant.ImageUrl}");
            }
        }
        async private Task RemoveGardenPlantAsync(int gardenId, int plantId, ListBoxItem selectedItem)
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                bool isRemoved = await uow.GardenPlantRepo.RemoveGardenPlantAsync(gardenId, plantId);
                if (isRemoved)
                {
                    await uow.CompleteAsync();
                    MessageBox.Show("Plant was succesfully removed!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    lstPlants.Items.Remove(selectedItem);
                }
            }
        }
    }
}
