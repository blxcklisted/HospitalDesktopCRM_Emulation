using Project.BLL.DTO;
using Project.BLL.Services;
using Project.UI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Project.UI.ViewModels
{
    internal class MakeAnAppointmentViewModel : BaseNotifyPropertyChanged
    {
        private ServiceStorage service;
        private UserDTO currentDoctor;
        private UserDTO currentUser;
        private AppointmentDTO selectedDate;
        private int selectedIndex;



        public UserDTO CurrentDoctor
        {
            get => currentDoctor;
            set => currentDoctor = value;
        }

        public AppointmentDTO SelectedDate
        {
            get => selectedDate;
            set
            {
                if (selectedDate != value)
                {
                    selectedDate = value;
                    NotifyOfPropertyChanged();
                }
            }
        }

        private List<AppointmentDTO> apps;

        public List<AppointmentDTO> Apps
        {
            get => apps;
            set
            {
                if (apps != value)
                {
                    apps = value;
                    NotifyOfPropertyChanged();
                }
            }
        }


        public MakeAnAppointmentViewModel(ServiceStorage service, UserDTO currentDoctor, UserDTO currentUser)
        {
            this.service = service;
            this.currentDoctor = currentDoctor;
            this.currentUser = currentUser;
            Fill();


            InitCommands();
        }

        private void Fill()
        {
            Apps = new List<AppointmentDTO>();
            Apps = service.AppointmentService.GetAll().Where(x => x.UserName == currentDoctor.Name && x.IsAppointed==false).ToList();

        }

        private void InitCommands()
        {
            ChooseDate = new RelayCommand(async x =>
            {
                try
                {
                    if (currentUser.AppointmentId != null)
                    {
                        MessageBox.Show("You've already made an Appointment");
                    }
                    else
                    {
                        if (selectedDate != null)
                        {
                            var dt = SelectedDate.DateTime;
                            var appId = service.AppointmentService.GetAll().Where(y => y.DateTime == dt).Select(y => y.Id).FirstOrDefault();
                            currentUser.AppointmentId = appId;

                            var app = service.AppointmentService.Get(appId);
                            app.IsAppointed = true;

                            await service.AppointmentService.UpdateAsync(app);
                            await service.UserService.UpdateAsync(currentUser);

                            MessageBox.Show("Success");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    throw;
                }
                finally
                {
                    Fill();
                }
            });
        }

        public ICommand ChooseDate { get; set; }
    }
}
