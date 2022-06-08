using Project.BLL.Services;
using Project.UI.Infrastructure;
using System;
using System.Linq;
using System.Web.Security;
using System.Windows;
using System.Windows.Input;

namespace Project.UI.ViewModels
{
    public class ResetPasswordViewModel
    {
        private readonly ServiceStorage service;
        private readonly LoginViewModel loginViewModel;

        public string Email { get; set; }
        public string NewPassword { get; set; }

        public ResetPasswordViewModel(ServiceStorage service, LoginViewModel loginViewModel)
        {
            this.service = service;
            this.loginViewModel = loginViewModel;
            InitCommands();
        }

        private void InitCommands()
        {
            BackCommand = new RelayCommand(x =>
            {
                loginViewModel.BackToLoginCommand.Execute(x);
            });
            ResetCommand = new RelayCommand(async x =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(Email))
                    {

                        if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            var user = service.UserService.GetAll().Where(y => y.Email == Email).FirstOrDefault();
                            if (user != null)
                            {
                                NewPassword = Membership.GeneratePassword(10, 0);
                                Clipboard.SetText(NewPassword);
                                MessageBox.Show("Your new Password was copied in your clipboard", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                                user.Password = new PasswordEncryptor(NewPassword).EncryptPassword();
                                await service.UserService.UpdateAsync(user);
                                BackCommand.Execute(x);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    throw;
                }
            });
        }

        public ICommand BackCommand { get; set; }
        public ICommand ResetCommand { get; set; }
    }
}
