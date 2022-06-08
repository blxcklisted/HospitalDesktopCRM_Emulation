using Project.BLL.Services;
using Project.UI.Infrastructure;
using System.Windows.Input;
using System.Linq;
using System.Windows;
using Project.BLL.DTO;
using System;

namespace Project.UI.ViewModels
{
    public class SingUpViewModel
    {
        private readonly ServiceStorage service;
        private readonly LoginViewModel loginViewModel;

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public SingUpViewModel(ServiceStorage service, LoginViewModel loginViewModel)
        {
            this.service = service;
            this.loginViewModel = loginViewModel;

            InitCommand();
        }

        private void InitCommand()
        {
            BackCommand = new RelayCommand(x =>
            {
                loginViewModel.BackToLoginCommand.Execute(x);
            });
            SingUpCommand = new RelayCommand(async x =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Email))
                    {
                        if (!CheckSameAccounts())
                        {
                            await service.UserService.CreateAsync(new UserDTO
                            {
                                Name = UserName,
                                Password = new PasswordEncryptor(Password).EncryptPassword(),
                                Email = Email,
                                RoleName = service.RoleService.GetAll().Where(y => y.Name == "User").Select(y => y.Name).FirstOrDefault()
                                
                            });
                            MessageBox.Show("Success!");
                            BackCommand.Execute(x);
                        }
                        else
                            MessageBox.Show("User with this User Name or Email is already exists");
                    }
                    else
                        MessageBox.Show("Fill all information");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    throw;
                }
            });
        }

        private bool CheckSameAccounts()
        {
            return service.UserService.GetAll().Where(x => x.Name == UserName || x.Email == Email).SingleOrDefault() != null;
        }

        public ICommand BackCommand { get; set; }
        public ICommand SingUpCommand { get; set; }

    }
}
