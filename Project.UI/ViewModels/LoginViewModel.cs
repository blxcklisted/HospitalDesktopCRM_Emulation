using Project.UI.Infrastructure;
using Project.UI.Views;
using Project.BLL.Services;
using System.Windows.Input;
using System.Windows.Controls;
using Project.DAL.Context;
using Project.DAL.Repositories;
using Project.BLL.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace Project.UI.ViewModels
{
    public class LoginViewModel : BaseNotifyPropertyChanged
    {
        private ApplicationDbContext dbContext;
        private RepositoryStorage repository;
        private ServiceStorage service;

        private UserDTO currentUser;
        private UserControl currentView;

        private string login;
        private string password;
        private string buttonBackVisibility;

        public string ButtonBackVisibility
        {
            get => buttonBackVisibility;
            set
            {
                buttonBackVisibility = value;
                NotifyOfPropertyChanged();
            }
        }

        public UserControl CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                NotifyOfPropertyChanged();
            }
        }
        public string Login
        {
            get => login;
            set
            {
                if (login != value)
                {
                    login = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    NotifyOfPropertyChanged();
                }
            }
        }

        public LoginViewModel()
        {
            InitializeDbComponents();
            InitCommands();
        }
        private void InitializeDbComponents()
        {
            dbContext = new ApplicationDbContext();
            repository = new RepositoryStorage(dbContext);
            service = new ServiceStorage(repository);
        }
        private void InitCommands()
        {
            BackToLoginCommand = new RelayCommand(x =>
            {
                CurrentView = null;
            });
            LoginCommand = new RelayCommand(async x =>
            {
                currentUser = await Task.Run(()=> SingInChecker());
                if (currentUser != null)
                { 
                    if(currentUser.RoleName == "Admin")
                    {
                        CurrentView = new AdminView();
                        CurrentView.DataContext = new AdminViewModel(service, this);
                    }
                    if(currentUser.RoleName == "User")
                    {
                        CurrentView = new UserView();
                        CurrentView.DataContext = new UserViewModel(service, currentUser, this);
                    }
                    if (currentUser.RoleName == "Doctor")
                    {
                        CurrentView = new DoctorView();
                        CurrentView.DataContext = new DoctorViewModel(service, currentUser,this);
                    }
                }
            });
            SingUpCommand = new RelayCommand(x =>
            {
                CurrentView = new SingUpView();
                CurrentView.DataContext = new SingUpViewModel(service, this);
            });
            ResetPassword = new RelayCommand(x =>
            {
                CurrentView = new ResetPasswordView();
                CurrentView.DataContext = new ResetPasswordViewModel(service, this);
            });
        }

        private UserDTO SingInChecker()
        {
            
            var encryptedPass = new PasswordEncryptor(Password).EncryptPassword();

            var user = service.UserService.GetAll()
                         .Where(x => x.Name == Login &&
                         x.Password == encryptedPass).SingleOrDefault();
            return user;
        }


        public ICommand LoginCommand { get; set; }
        public ICommand BackToLoginCommand { get; set; }
        public ICommand BackToUserViewCommand { get; set; }
        public ICommand SingUpCommand { get; set; }
        public ICommand ResetPassword { get; set; }

    }

}
