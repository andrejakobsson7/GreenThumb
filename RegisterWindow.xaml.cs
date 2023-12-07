using GreenThumb.Database;
using GreenThumb.Database.Repositories;
using GreenThumb.Models;
using System.Windows;

namespace GreenThumb
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            RedirectToLoginPage();
        }

        async private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            bool isValidUserInfo = ValidateUsernameAndPassword(txtUsername.Text, pbPassword.Password);
            if (isValidUserInfo)
            {
                using (AppDbContext context = new())
                {
                    GreenThumbUow uow = new(context);
                    var foundUser = await uow.UserRepo.GetUserByUsernameAsync(txtUsername.Text);
                    if (foundUser == null)
                    {
                        UserModel newUser = new(txtUsername.Text, pbPassword.Password);
                        await uow.UserRepo.AddUserAsync(newUser);
                        await uow.CompleteAsync();
                        GardenModel newGarden = new($"{newUser.Username}'s garden", newUser.UserId);
                        await uow.GardenRepo.AddGardenAsync(newGarden);
                        await uow.CompleteAsync();
                        ConfirmRegistration(newUser.Username);
                        RedirectToLoginPage();
                    }
                    else
                    {
                        MessageBox.Show("Username is already in use, try another!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }
        private bool ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("No username has been entered!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private bool ValidatePassword(string password)
        {
            if (password.Length < 5)
            {
                MessageBox.Show("Password needs to be at least 5 characters long", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private bool ValidateUsernameAndPassword(string username, string password)
        {
            if (ValidateUsername(username) && ValidatePassword(password))
            {
                return true;
            }
            return false;
        }
        private void ConfirmRegistration(string username)
        {
            MessageBox.Show($"{username} was successfully registered as a new member! You will now be redirected to login page", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void RedirectToLoginPage()
        {
            MainWindow mw = new();
            mw.Show();

            Close();
        }


    }
}
