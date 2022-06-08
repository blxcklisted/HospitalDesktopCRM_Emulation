using Project.BLL.DTO;
using Project.BLL.Services;
using Project.UI.Infrastructure;
using Project.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project.UI.ViewModels
{
    public class UserViewModel : BaseNotifyPropertyChanged
    {
        private ServiceStorage service;

        private LoginViewModel loginViewModel;
        private UserDTO currentUser;
        private UserControl currentView;
        private DrugDTO selectedDrug;
        private UserDTO selectedDoctor;

       


        public IEnumerable<DrugDTO> Drugs { get; set; }
        public IEnumerable<UserDTO> Doctors { get; set; }

        public UserControl CurrentView
        {
            get => currentView;
            set
            {
                if (currentView != value)
                {
                    currentView = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        public DrugDTO SelectedDrug
        {
            get => selectedDrug;
            set
            {
                if (selectedDrug != value)
                {
                    selectedDrug = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        public UserDTO SelectedDoctor
        {
            get => selectedDoctor;
            set
            {
                if (selectedDoctor != value)
                {
                    selectedDoctor = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        public UserViewModel(ServiceStorage service, UserDTO currentUser, LoginViewModel loginViewModel)
        {
            this.service = service;
            this.currentUser = currentUser;
            this.loginViewModel = loginViewModel;
            Drugs = new ObservableCollection<DrugDTO>();
            Doctors = new ObservableCollection<UserDTO>();
            FillCollections();
            InitCommands();
        }

        private void InitCommands()
        {
            BackToLoginCommand = new RelayCommand(x =>
            {
                loginViewModel.BackToLoginCommand.Execute(x);

            });
            BuyDrugCommand = new RelayCommand(x =>
            {
                CurrentView = new BuyDrugView();
                CurrentView.DataContext = new BuyDrugViewModel(service, SelectedDrug, currentUser, this);
            });
            ShowAppointments = new RelayCommand(x =>
            {
                CurrentView = new MakeAnAppointmentView();
                CurrentView.DataContext = new MakeAnAppointmentViewModel(service, selectedDoctor, currentUser);
            });
            ShowProfileCommand = new RelayCommand(x =>
            {
                CurrentView = new UserProfileView();
                CurrentView.DataContext = new UserProfileViewModel(service, currentUser);
            });

        }

        private void FillCollections()
        {
            Drugs = service.DrugService.GetAll();
            Doctors = service.UserService.GetAll().Where(x => x.RoleName == "Doctor");
        }

        public ICommand BuyDrugCommand { get; set; }
        public ICommand BackToLoginCommand { get; set; }

        public ICommand ShowProfileCommand { get; set; }
        public ICommand ShowAppointments { get; set; }
        public ICommand BackToUserViewCommand { get; set; }

    }
}
