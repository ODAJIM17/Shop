

namespace Shop.UIForms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Shop.Common.Models;
    using Shop.Common.Services;
    using Shop.UIForms.Views;
    using System.Windows.Input;
    using Xamarin.Forms;

    
    public class LoginViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand => new RelayCommand(this.Login);

        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.Email = "davila56jd@gmail.com";
            this.Password = "123456";
            this.isEnabled = true;
        }


        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error", 
                    "Please enter your email",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Please enter a password", 
                    "Accept");
                return;
            }

            if (!this.Email.Equals("davila56jd@gmail.com") || !this.Password.Equals("123456"))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Incorrect user or password", 
                    "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetTokenAsync(
                url,
                "/Account",
                "/CreateToken",
                request);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Email or password incorrect.",
                    "Accept");
                return;
            }

            var token = (Common.Models.TokenResponse)response.Result;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.Products = new ProductsViewModel();
            Application.Current.MainPage = new MasterPage();

        }
    }
}
