using Project.BLL.DTO;
using Project.BLL.Services;
using Project.UI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Project.UI.ViewModels
{
    internal class UserProfileViewModel : BaseNotifyPropertyChanged
    {
        private ServiceStorage service;
        private string userAppointmentDateTime;

        public UserDTO CurrentUser { get; set; }
        public string NameToChange { get; set; }
        public string PasswordToChange { get; set; }
        public string EmailToChange { get; set; }

        public string UserAppointmentDateTime
        {
            get => userAppointmentDateTime;
            set
            {
                if (userAppointmentDateTime != value)
                {
                    userAppointmentDateTime = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        public UserProfileViewModel(ServiceStorage service, UserDTO currentUser)
        {
            this.service = service;
            CurrentUser = currentUser;
            NameToChange = currentUser.Name;
            EmailToChange = currentUser.Email;
            PasswordToChange = currentUser.Password;

            InitCommands();
            Fill();
        }

        private void InitCommands()
        {

            CancelAppointmentCommand = new RelayCommand(x =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(UserAppointmentDateTime))
                    {
                        if (MessageBox.Show("Are You sure?", "Canceling the Appointment", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            var app = service.AppointmentService.Get(CurrentUser.AppointmentId.Value);
                            app.IsAppointed = false;
                            CurrentUser.AppointmentId = null;

                            service.AppointmentService.UpdateAsync(app);
                            service.UserService.UpdateAsync(CurrentUser);

                            UserAppointmentDateTime = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Question);
                    throw;
                }

            });
            ChangeNameCommand = new RelayCommand(x =>
            {
                try
                {
                    if (CurrentUser.Name != NameToChange)
                    {
                        if (service.UserService.GetAll().FirstOrDefault(y => y.Name == NameToChange) == null)
                        {
                            CurrentUser.Name = NameToChange;
                            service.UserService.UpdateAsync(CurrentUser);
                            MessageBox.Show("Success", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            MessageBox.Show("User with the same Name is already exists", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Question);
                    throw;
                }
            });
            ChangeEmailCommand = new RelayCommand(x =>
            {
                try
                {
                    if (CurrentUser.Email != EmailToChange)
                    {
                        if (service.UserService.GetAll().FirstOrDefault(y => y.Email == EmailToChange) == null)
                        {
                            CurrentUser.Email = EmailToChange;
                            service.UserService.UpdateAsync(CurrentUser);
                            MessageBox.Show("Success", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            MessageBox.Show("User with the same Email is already exists", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Question);
                    throw;
                }
            });
            ChangePasswordCommand = new RelayCommand(x =>
            {
                try
                {
                    if (CurrentUser.Password != new PasswordEncryptor(PasswordToChange).EncryptPassword())
                    {
                        CurrentUser.Password = new PasswordEncryptor(PasswordToChange).EncryptPassword();
                        service.UserService.UpdateAsync(CurrentUser);
                        MessageBox.Show("Success", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Question);
                    throw;
                }
            });
        }

        private void Fill()
        {
            UserAppointmentDateTime = service.AppointmentService.GetAll().Where(x => x.Id == CurrentUser.AppointmentId).Select(x => x.DateTime.ToString()).FirstOrDefault();
        }

        public ICommand CancelAppointmentCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand ChangeEmailCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
    }
}
