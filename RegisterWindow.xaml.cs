using GreenThumb.Database;
using GreenThumb.Database.Repositories;
using GreenThumb.Managers;
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
            LoadLogo();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            RedirectToLoginPage();
        }

        async private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            bool isValidUsername = await ValidateUsername(txtUsername.Text);
            if (isValidUsername)
            {
                bool isValidPassword = ValidatePassword(pbPassword.Password);
                if (isValidPassword)
                {
                    UserModel newUser = new(txtUsername.Text, pbPassword.Password);
                    await AddUserAsync(newUser);
                    ConfirmRegistration(newUser.Username);
                    RedirectToLoginPage();
                    //When a user is added there is a trigger in SQL creating a garden for the user
                }
            }

        }

        private void LoadLogo()
        {
            imgLogo.Source = ImageManager.GetLogo(imgLogo.Source);
        }

        async private Task<bool> ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Invalid username!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return await CheckIfUsernameIsTaken(username);
        }
        async private Task<bool> CheckIfUsernameIsTaken(string username)
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                var user = await uow.UserRepo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return true;
                }
                MessageBox.Show("Username is already in use, try another!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
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

        async private Task AddUserAsync(UserModel newUser)
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                await uow.UserRepo.AddUserAsync(newUser);
                await uow.CompleteAsync();
            }
        }
        private void ConfirmRegistration(string username)
        {
            MessageBox.Show($"{username} was successfully registered as a new customer! You will now be redirected to login page", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void RedirectToLoginPage()
        {
            MainWindow mw = new();
            mw.Show();

            Close();
        }


    }
}
