using Project.BLL.DTO;
using Project.BLL.Services;
using Project.UI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Project.UI.ViewModels
{
    public class DoctorViewModel : BaseNotifyPropertyChanged
    {
        private ServiceStorage service;
        private UserDTO currentDoctor;
        private LoginViewModel loginViewModel;
        private AppointmentDTO selectedAppointment;
        private int hours;
        private int minutes;
        private IEnumerable<AppointmentDTO> appointments;

        public int Hours
        {
            get => hours;
            set
            {
                if (value >= 0 && value <= 23)
                {
                    hours = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        public int Minutes
        {
            get => minutes;
            set
            {
                if (value >= 0 && value <= 59)
                {
                    minutes = value;
                    NotifyOfPropertyChanged();
                }
            }
        }

        public AppointmentDTO SelectedAppointment
        {
            get => selectedAppointment;
            set
            {
                if (selectedAppointment != value)
                {
                    selectedAppointment = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        public DateTime SelectedDate { get; set; }
        public DateTime Today { get; set; } = DateTime.Now;
        public IEnumerable<AppointmentDTO> Appointments 
        { 
            get => appointments; 
            set
            {
                appointments = value;
                NotifyOfPropertyChanged();
            }
        }
        public DoctorViewModel(ServiceStorage service, UserDTO currentDoctor, LoginViewModel loginViewModel)
        {
            this.service = service;
            this.currentDoctor = currentDoctor;
            this.loginViewModel = loginViewModel;
            Hours = DateTime.Now.Hour;
            Minutes = DateTime.Now.Minute;
            Today = DateTime.Now;
            Fill();

            InitCommand();
        }
        private void Fill()
        {
            Appointments = service.AppointmentService.GetAll().Where(x=>x.UserName == currentDoctor.Name);
        }
        private void InitCommand()
        {
            CreateAppointmentCommand = new RelayCommand(x =>
            {
                try
                {
                    string datetime = $"{SelectedDate.Day}.{SelectedDate.Month}.{SelectedDate.Year} {hours}:{minutes}:00";
                    var dt = DateTime.Parse(datetime);
                    var app = new AppointmentDTO { DateTime = dt, UserName = currentDoctor.Name };
                    service.AppointmentService.CreateAsync(app);
                    MessageBox.Show("Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    throw;
                }
                finally
                {
                    BackToLoginCommand.Execute(x);
                }
            });
            DeleteAppointmentCommand = new RelayCommand(x =>
            {
                try
                {
                    if (SelectedAppointment != null)
                    {
                        if (MessageBox.Show("Are You sure?", "Submition", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            var app = service.AppointmentService.Get(SelectedAppointment.Id);
                            var user = service.UserService.GetAll().Where(y => y.AppointmentId == app.Id).FirstOrDefault();
                            if (user != null)
                            {
                                if (user.AppointmentId != null)
                                {
                                    user.AppointmentId = null;
                                    service.AppointmentService.DeleteAsync(app);
                                    service.UserService.UpdateAsync(user);
                                }
                            }
                            else
                                service.AppointmentService.DeleteAsync(app);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    throw;
                }
                finally
                {
                    Fill();
                }
            });
            BackToLoginCommand = new RelayCommand(x =>
            {
                loginViewModel.BackToLoginCommand.Execute(x);
            });
            RefreshGridCommand = new RelayCommand(x =>
            {
                Appointments = service.AppointmentService.GetAll().Where(y => y.UserName == currentDoctor.Name);
            });
        }

        public ICommand CreateAppointmentCommand { get; set; }
        public ICommand DeleteAppointmentCommand { get; set; }
        public ICommand BackToLoginCommand { get; set; }
        public ICommand RefreshGridCommand { get; set; }

    }
}
