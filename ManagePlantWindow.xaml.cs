using GreenThumb.Database;
using GreenThumb.Database.Repositories;
using GreenThumb.Managers;
using GreenThumb.Models;
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
        //Adding new plant
        public ManagePlantWindow()
        {
            InitializeComponent();
            LoadLogo();
        }

        //See details/update existing plant
        public ManagePlantWindow(PlantModel plantToDisplay)
        {
            InitializeComponent();
            PlantToDisplay = plantToDisplay;
            LoadLogo();
            LoadAllPlantInformation();
            ActivateReadOnlyMode();
            //Load in information about the passed along plant

            //Enable "edit"-button, if clicked - enable all fields and enable save-button.
            //Enable button to add plant to "my garden".
        }

        private void LoadLogo()
        {
            imgLogo.Source = ImageManager.GetLogo(imgLogo.Source);
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
        }
        private void LoadAllPlantInformation()
        {
            txtPlantName.Text = PlantToDisplay.Name;
            dpPlantDate.SelectedDate = PlantToDisplay.PlantDate;
            foreach (var instruction in PlantToDisplay.Instructions)
            {
                ListBoxItem item = new();
                item.Tag = instruction;
                item.Content = instruction.ToString();
                lstCareInstructions.Items.Add(item);
            }

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

        private void btnRemoveCareInstruction_Click(object sender, RoutedEventArgs e)
        {
            bool isValidInstruction = ValidateCareInstructionHasBeenSelected(lstCareInstructions.SelectedItem);
            if (isValidInstruction)
            {
                lstCareInstructions.Items.Remove(lstCareInstructions.SelectedItem);
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
        private bool ValidateCareInstructionHasBeenSelected(object selectedItem)
        {
            if (selectedItem == null)
            {
                MessageBox.Show("No care instruction has been selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("No instructions has been entered!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        await uow.PlantRepo.AddPlantAsync(newPlant);
                        await uow.CompleteAsync();
                        foreach (var plant in lstCareInstructions.Items)
                        {
                            ListBoxItem item = (ListBoxItem)plant;
                            InstructionModel newInstruction = new(item.Content.ToString()!, newPlant.PlantId);
                            await uow.InstructionRepo.AddInstructionAsync(newInstruction);
                        }
                        await uow.CompleteAsync();
                        MessageBox.Show("Plant was successfully registered!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //Update
                        var plantToUpdate = await uow.PlantRepo.GetPlantByIdAsync(PlantToDisplay.PlantId);
                        if (plantToUpdate != null)
                        {
                            plantToUpdate.Name = txtPlantName.Text;
                            plantToUpdate.PlantDate = (DateTime)dpPlantDate.SelectedDate;
                            //Delete all instructions
                            await uow.InstructionRepo.RemoveInstructionsByPlantId(PlantToDisplay.PlantId);
                            await uow.CompleteAsync();
                            //Add them again
                            foreach (var plant in lstCareInstructions.Items)
                            {
                                ListBoxItem item = (ListBoxItem)plant;
                                InstructionModel newInstruction = new(item.Content.ToString()!, plantToUpdate.PlantId);
                                await uow.InstructionRepo.AddInstructionAsync(newInstruction);
                            }
                            await uow.CompleteAsync();
                            MessageBox.Show("Plant was successfully updated!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);

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
        }
    }
}
