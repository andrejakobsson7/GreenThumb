using GreenThumb.Database;
using GreenThumb.Database.Repositories;
using GreenThumb.Managers;
using GreenThumb.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace GreenThumb
{
    /// <summary>
    /// Interaction logic for ManagePlantWindow.xaml
    /// </summary>
    public partial class ManagePlantWindow : Window
    {
        public PlantModel? PlantToDisplay { get; set; }
        private string? TemporaryFileNameHolder { get; set; }


        public ManagePlantWindow()
        {
            //Adding new plant
            InitializeComponent();
        }

        public ManagePlantWindow(PlantModel plantToDisplay)
        {
            //See details/update existing plant
            InitializeComponent();
            PlantToDisplay = plantToDisplay;
            LoadAllPlantInformation();
            ActivateReadOnlyMode();

        }
        private void LoadAllPlantInformation()
        {
            txtPlantName.Text = PlantToDisplay!.Name;
            dpPlantDate.SelectedDate = PlantToDisplay.PlantDate;
            imgPlantImage.Source = ImageManager.GetPlantImage(PlantToDisplay.ImageUrl);
            foreach (var instruction in PlantToDisplay.Instructions)
            {
                ListBoxItem item = new();
                item.Tag = instruction;
                item.Content = instruction.ToString();
                lstCareInstructions.Items.Add(item);
            }

        }

        private void ActivateReadOnlyMode()
        {
            lblAction.Content = "Plant details";
            lblCareInstruction.Visibility = Visibility.Collapsed;
            txtPlantName.IsEnabled = false;
            txtCareInstruction.Visibility = Visibility.Collapsed;
            btnAddCareInstruction.Visibility = Visibility.Collapsed;
            btnRemoveCareInstruction.Visibility = Visibility.Collapsed;
            dpPlantDate.IsEnabled = false;
            btnSave.Visibility = Visibility.Collapsed;
            btnEdit.Visibility = Visibility.Visible;
            btnAddToMyGarden.Visibility = Visibility.Visible;
            btnAddPlantImage.Visibility = Visibility.Collapsed;
        }


        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            RedirectToPlantWindow();
        }

        private void RedirectToPlantWindow()
        {
            PlantToDisplay = null;
            PlantWindow pw = new();
            pw.Show();
            Close();
        }

        private void btnAddCareInstruction_Click(object sender, RoutedEventArgs e)
        {
            bool isValidInstruction = ValidateInstructionInput(txtCareInstruction.Text);
            if (isValidInstruction)
            {
                CreateAndAddListBoxItem(txtCareInstruction.Text);
                ClearInstructionTextField();
            }

        }
        private bool ValidateInstructionInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("No care instruction has been entered!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void CreateAndAddListBoxItem(string description)
        {
            ListBoxItem item = new();
            item.Tag = description;
            item.Content = description;
            lstCareInstructions.Items.Add(item);
        }

        private void btnRemoveCareInstruction_Click(object sender, RoutedEventArgs e)
        {
            bool isValidInstruction = ValidateCareInstructionHasBeenSelected(lstCareInstructions.SelectedItem);
            if (isValidInstruction)
            {
                lstCareInstructions.Items.Remove(lstCareInstructions.SelectedItem);
            }
        }


        private bool ValidateCareInstructionHasBeenSelected(object selectedItem)
        {
            if (selectedItem == null)
            {
                MessageBox.Show("No care instruction has been selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool ValidatePlantName(string plantName)
        {
            if (string.IsNullOrWhiteSpace(plantName))
            {
                MessageBox.Show("No plant name has been entered!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private bool ValidateInstructionsHasBeenEntered(ListBox listBoxToCheck)
        {
            if (listBoxToCheck.Items.Count <= 0)
            {
                MessageBox.Show("No instructions have been entered!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool ValidateDateHasBeenSelected(DatePicker datePickerToCheck)
        {
            if (datePickerToCheck.SelectedDate == null)
            {
                MessageBox.Show("No plant date has been entered!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool ValidateAllInput(string plantName, ListBox listBoxToCheck, DatePicker datePickerToCheck)
        {
            if (ValidatePlantName(plantName)
                && ValidateInstructionsHasBeenEntered(listBoxToCheck)
                && ValidateDateHasBeenSelected(datePickerToCheck))
            {
                return true;
            }
            return false;
        }

        async private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool isAllInputValid = ValidateAllInput(txtPlantName.Text, lstCareInstructions, dpPlantDate);
            if (isAllInputValid)
            {
                PlantModel newPlant = new(txtPlantName.Text, (DateTime)dpPlantDate.SelectedDate!);
                using (AppDbContext context = new())
                {
                    GreenThumbUow uow = new(context);
                    if (PlantToDisplay == null)
                    {
                        //Add new
                        //Check if plant exists with that name.
                        var foundPlant = await uow.PlantRepo.GetPlantByName(newPlant.Name);
                        if (foundPlant == null)
                        {
                            await uow.PlantRepo.AddPlantAsync(newPlant);
                            await uow.CompleteAsync();
                            foreach (var instruction in lstCareInstructions.Items)
                            {
                                ListBoxItem item = (ListBoxItem)instruction;
                                InstructionModel newInstruction = new(item.Content.ToString()!, newPlant.PlantId);
                                await uow.InstructionRepo.AddInstructionAsync(newInstruction);
                            }
                            await uow.CompleteAsync();
                            //Here we insert url for the plants' image and add it to the projects folder if the user has added an image
                            if (TemporaryFileNameHolder != null)
                            {
                                newPlant.ImageUrl = ImageManager.GetImageUrl();
                                await uow.CompleteAsync();
                                ImageManager.AddImageToFolder(TemporaryFileNameHolder, newPlant.ImageUrl);
                            }
                            MessageBox.Show("Plant was successfully registered!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                            UpdateUi();
                        }
                        else
                        {
                            MessageBox.Show($"Plant with name {newPlant.Name} already exists, please choose another name!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    else
                    {
                        //Update
                        var plantToUpdate = await uow.PlantRepo.GetPlantByIdAsync(PlantToDisplay.PlantId);
                        if (plantToUpdate != null)
                        {
                            plantToUpdate.Name = newPlant.Name;
                            plantToUpdate.PlantDate = newPlant.PlantDate;
                            //Delete all instructions
                            await uow.InstructionRepo.RemoveInstructionsByPlantId(PlantToDisplay.PlantId);
                            await uow.CompleteAsync();
                            //Add them again
                            foreach (var instruction in lstCareInstructions.Items)
                            {
                                ListBoxItem item = (ListBoxItem)instruction;
                                InstructionModel newInstruction = new(item.Content.ToString()!, plantToUpdate.PlantId);
                                await uow.InstructionRepo.AddInstructionAsync(newInstruction);
                            }
                            await uow.CompleteAsync();
                            //If the user has added a new image, we update the plant's image url to the new information.
                            //Haven't found a 100% way to remove the previous image from the project folder.
                            if (TemporaryFileNameHolder != null)
                            {
                                plantToUpdate.ImageUrl = ImageManager.GetImageUrl();
                                await uow.CompleteAsync();
                                ImageManager.AddImageToFolder(TemporaryFileNameHolder, plantToUpdate.ImageUrl);
                            }
                            MessageBox.Show("Plant was successfully updated. You will now be redirected to home page", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                            ResetPlantToDisplay();
                            ResetTemporaryFileNameHolder();
                            RedirectToPlantWindow();
                        }
                    }
                }
            }

        }

        private void UpdateUi()
        {
            txtPlantName.Text = "";
            ClearInstructionTextField();
            lstCareInstructions.Items.Clear();
            dpPlantDate.SelectedDate = null;
            imgPlantImage.Source = null;
            ResetTemporaryFileNameHolder();
        }

        private void ResetPlantToDisplay()
        {
            PlantToDisplay = null;
        }
        private void ResetTemporaryFileNameHolder()
        {
            TemporaryFileNameHolder = null;
        }

        private void ClearInstructionTextField()
        {
            txtCareInstruction.Text = "";
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            lblCareInstruction.Visibility = Visibility.Visible;
            txtPlantName.IsEnabled = true;
            txtCareInstruction.Visibility = Visibility.Visible;
            btnAddCareInstruction.Visibility = Visibility.Visible;
            btnRemoveCareInstruction.Visibility = Visibility.Visible;
            dpPlantDate.IsEnabled = true;
            btnEdit.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
            btnAddToMyGarden.Visibility = Visibility.Collapsed;
            btnAddPlantImage.Visibility = Visibility.Visible;
        }

        async private void btnAddToMyGarden_Click(object sender, RoutedEventArgs e)
        {
            //Since this option only is available when displaying an existing plant, no validation is required.
            GardenPlant newGardenPlant = new(UserManager.SignedInUser!.Garden!.GardenId, PlantToDisplay!.PlantId);
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                await uow.GardenPlantRepo.AddGardenPlantAsync(newGardenPlant);
                try
                {
                    await uow.CompleteAsync();
                    MessageBox.Show($"{PlantToDisplay.Name} was successfully added to {UserManager.SignedInUser!.Garden.GardenName}!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                {
                    MessageBox.Show($"{PlantToDisplay.Name} already exists in {UserManager.SignedInUser!.Garden.GardenName} and cannot be added again!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Something unexpected went wrong, please contact your system administrator\n{ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAddPlantImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg;*.jpeg;*.png;*.pdf) | *.jpg;*.jpeg;*.png;*.pdf | All files (*.*) | *.*";
            bool? userOption = ofd.ShowDialog();
            if (userOption == true)
            {
                //Save file as image source so it can be viewed in the UI directly
                imgPlantImage.Source = ImageManager.GetPlantImage(ofd.FileName);
                //Save it temporarily in the field variable so it can be accessed later on when saving
                TemporaryFileNameHolder = ofd.FileName;
            }
        }
    }
}
