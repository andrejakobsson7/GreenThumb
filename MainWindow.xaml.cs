using GreenThumb.Database;
using GreenThumb.Database.Repositories;
using GreenThumb.Managers;
using GreenThumb.Models;
using System.Windows;

namespace GreenThumb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadLogo();
        }

        private void LoadLogo()
        {
            imgLogo.Source = ImageManager.GetLogo(imgLogo.Source);
        }

        private async void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            var userToSignIn = await SignIn(txtUsername.Text, pbPassword.Password);
            if (userToSignIn != null)
            {
                RedirectToPlantWindow(userToSignIn);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow rW = new();
            rW.Show();

            Close();
        }

        async private Task<UserModel?> SignIn(string username, string password)
        {
            using (AppDbContext context = new())
            {
                GreenThumbUow uow = new(context);
                var userToSignIn = await uow.UserRepo.SignInUser(username, password);
                if (userToSignIn != null)
                {
                    return userToSignIn;
                }
                MessageBox.Show("Invalid username and/or password, try again!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void RedirectToPlantWindow(UserModel userToSignIn)
        {
            PlantWindow pw = new(userToSignIn);
            pw.Show();

            Close();
        }
    }
}