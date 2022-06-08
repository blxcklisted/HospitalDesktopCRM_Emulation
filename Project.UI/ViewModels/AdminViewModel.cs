using Project.UI.Infrastructure;
using Project.UI.Views;
using Project.BLL.DTO;
using Project.BLL.Services;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace Project.UI.ViewModels
{
    public class AdminViewModel : BaseNotifyPropertyChanged
    {
        private ServiceStorage service;
        private LoginViewModel loginViewModel;



        #region RoleTabFieldsAndProp
        public string NewRoleName { get; set; }
        #endregion

        #region UsersTabFieldsAndProp
        private UserDTO selectedRow;
        private string filePath;

        private IEnumerable<UserDTO> users;
        private List<string> roles;
        private string comboboxRole;

        public string FilePath
        {
            get => filePath;
            set
            {
                if (value != filePath)
                {
                    filePath = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        public string ComboboxManufacturer { get; set; }
        public string ComboboxCategory { get; set; }
        public List<string> ManufacturersNamesList { get; set; }
        public List<string> CategoriesNamesList { get; set; }
        public string ComboboxRole
        {
            get => comboboxRole;
            set
            {
                if (comboboxRole != value)
                {
                    comboboxRole = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        public List<string> Roles
        {
            get => roles;
            set
            {
                if (roles != value)
                {
                    roles = value;
                    NotifyOfPropertyChanged();
                }

            }
        }
        public UserDTO SelectedRow
        {
            get => selectedRow;
            set
            {
                if (selectedRow != value)
                {
                    selectedRow = value;
                    NotifyOfPropertyChanged();
                }
            }
        }


        public IEnumerable<UserDTO> Users
        {
            get => users;
            set
            {
                if (users != value)
                {
                    users = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        #endregion

        #region CategoryTabFieldsAndProp

        private CategoryDTO selectedCategoryRow;


        private IEnumerable<CategoryDTO> categories;


        public string NewCategoryName { get; set; }
        public IEnumerable<CategoryDTO> Categories
        {
            get => categories;
            set
            {
                if (categories != value)
                {
                    categories = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        public CategoryDTO SelectedCategoryRow
        {
            get => selectedCategoryRow;
            set
            {
                if (selectedCategoryRow != value)
                {
                    selectedCategoryRow = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        #endregion

        #region ManufacturerTabFieldsAndProp

        private IEnumerable<ManufacturerDTO> manufacturers;
        private ManufacturerDTO selectedManufacturerRow;

        public string NewManufacturerName { get; set; }
        public string UpdateManufacturerName { get; set; }

        public IEnumerable<ManufacturerDTO> Manufacturers
        {
            get => manufacturers;
            set
            {
                if (manufacturers != value)
                {
                    manufacturers = value;
                    NotifyOfPropertyChanged();
                }
            }
        }

        public ManufacturerDTO SelectedManufacturerRow
        {
            get => selectedManufacturerRow;
            set
            {
                if (selectedManufacturerRow != value)
                {
                    selectedManufacturerRow = value;
                    NotifyOfPropertyChanged();
                }
            }
        }
        #endregion

        #region DrugTabFieldsAndProp

        public string NewDrugName { get; set; }

        private DrugDTO selectedDrugRow;

        private BitmapImage image;


        public int DrugCount { get; set; }
        public decimal DrugPrice { get; set; }
        public BitmapImage Image
        {
            get => image;
            set
            {
                if (image != value)
                {
                    image = value;
                    NotifyOfPropertyChanged();
                }
            }
        }

        public DrugDTO SelectedDrugRow
        {
            get => selectedDrugRow;
            set
            {
                if (selectedDrugRow != value)
                {
                    selectedDrugRow = value;
                    ShowImage.Execute(selectedDrugRow);
                    NotifyOfPropertyChanged();
                }
            }
        }
        private IEnumerable<DrugDTO> drugs;

        public IEnumerable<DrugDTO> Drugs
        {
            get => drugs;
            set
            {
                if (drugs != value)
                {
                    drugs = value;
                    NotifyOfPropertyChanged();
                }
            }
        }

        #endregion



        public AdminViewModel(ServiceStorage service, LoginViewModel loginViewModel)
        {
            InitProperties();
            this.service = service;
            this.loginViewModel = loginViewModel;
            InitCommands();
            FillUserRoleCombobox();
            FillManufacturersAndCategories();
            ComboboxRole = Roles[0];
            ComboboxManufacturer = ManufacturersNamesList[0];
            ComboboxCategory = CategoriesNamesList[0];

        }

        private void InitProperties()
        {
            Roles = new List<string>();
            ManufacturersNamesList = new List<string>();
            CategoriesNamesList = new List<string>();
        }

        private void InitCommands()
        {

            AddRoleCommand = new RelayCommand(async x => await service.CategoryService.CreateAsync(new CategoryDTO { Name = NewRoleName }));
            ShowUsersDataCommand = new RelayCommand(x =>
            {
                Users = service.UserService.GetAll();
            });
            UpdateUserRole = new RelayCommand(async x =>
            {
                if (SelectedRow != null)
                {
                    if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) ;
                    {
                        var user = service.UserService.Get(selectedRow.Id);
                        user.RoleName = ComboboxRole;
                        await service.UserService.UpdateAsync(user);
                        ShowUsersDataCommand.Execute(x);
                    }
                }
            });

            ShowImage = new RelayCommand(x =>
            {
                Image = ByteArrayConverter.GetBitmapImage(selectedDrugRow?.Image);
            });
            ShowDrugDataCommand = new RelayCommand(x =>
            {
                Drugs = service.DrugService.GetAll();
            });
            AddNewDrugCommand = new RelayCommand(x =>
            {
                if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var drug = new DrugDTO
                    {
                        Name = NewDrugName,
                        CategoryName = ComboboxCategory,
                        ManufacturerName = ComboboxManufacturer,
                        DrugCount = DrugCount,
                        DrugPrice = DrugPrice
                    };
                    service.DrugService.CreateAsync(drug);
                }
            });
            UpdateDrugCommand = new RelayCommand(async x =>
            {
                if(MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await service.DrugService.UpdateAsync(SelectedDrugRow);
                }
            });
            OpenFileDialogCommand = new RelayCommand(x =>
            {
                OpenFileDialog dlg = new OpenFileDialog();
                if (dlg.ShowDialog() == true)
                {
                    FilePath = dlg.FileName;
                    InsertImageInSelectedCommand.Execute(x);
                }
                ShowDrugDataCommand.Execute(x);
            });
            InsertImageInSelectedCommand = new RelayCommand(x =>
            {
                if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        var drg = service.DrugService.Get(SelectedDrugRow.Id);
                        drg.Image = ByteArrayConverter.GetByteArray(FilePath);
                        service.DrugService.UpdateAsync(drg);
                    }
                }
            });
            DeleteDrugCommand = new RelayCommand(async x =>
            {
                if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var drug = service.DrugService.Get(selectedDrugRow.Id);

                    await service.DrugService.DeleteAsync(drug);
                    ShowUsersDataCommand.Execute(x);

                }
            });

            AddNewManufacturerCommand = new RelayCommand(x =>
            {
                var man = new ManufacturerDTO
                {
                    Name = NewManufacturerName
                };
                service.ManufacturerService.CreateAsync(man);
            });
            UpdateManufacturerCommand = new RelayCommand(x =>
            {
                if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    service.ManufacturerService.UpdateAsync(SelectedManufacturerRow);
                }
            });
            DeleteManufacturerCommand = new RelayCommand(x =>
            {
                if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var man = service.ManufacturerService.Get(SelectedManufacturerRow.Id);
                    service.ManufacturerService.DeleteAsync(man);
                }
            });
            ShowManufacturerDataCommand = new RelayCommand(x =>
            {
                Manufacturers = service.ManufacturerService.GetAll();
            });


            ShowCategoryDataCommand = new RelayCommand(x =>
            {
                Categories = service.CategoryService.GetAll();
            });

            DeleteCategoryCommand = new RelayCommand(x =>
            {
                if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var cat = service.CategoryService.Get(SelectedCategoryRow.Id);
                    service.CategoryService.DeleteAsync(cat);
                }
            });
            AddNewCategoryCommand = new RelayCommand(x =>
            {
                if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var cat = new CategoryDTO
                    {
                        Name = NewCategoryName
                    };
                    service.CategoryService.CreateAsync(cat);
                }
            });
            UpdateCategoryCommand = new RelayCommand(x =>
            {
                if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    service.CategoryService.UpdateAsync(SelectedCategoryRow);
                }
            });
            BackCommand = new RelayCommand(x =>
            {
                loginViewModel.BackToLoginCommand.Execute(x);
            });
        }


        #region RoleTabCommandsAndMehtods
        public ICommand AddRoleCommand { get; set; }

        #endregion

        #region UsersTabCommandsAndMehtods
        private void FillUserRoleCombobox()
        {
            var rol = service.RoleService.GetAll().ToList();
            rol.ForEach(x => Roles.Add(x.Name));
        }

        public ICommand ShowUsersDataCommand { get; set; }
        public ICommand UpdateUserRole { get; set; }


        #endregion

        #region CategoryTabCommandsAndMehtods
        public ICommand ShowCategoryDataCommand { get; set; }
        public ICommand AddNewCategoryCommand { get; set; }
        public ICommand UpdateCategoryCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }
        #endregion

        #region ManufacturerTabCommandsAndMehtods

        public ICommand AddNewManufacturerCommand { get; set; }
        public ICommand UpdateManufacturerCommand { get; set; }
        public ICommand DeleteManufacturerCommand { get; set; }

        public ICommand ShowManufacturerDataCommand { get; set; }
        #endregion

        #region DrugTabCommandsAndMehtods

        private void FillManufacturersAndCategories()
        {
            var man = service.ManufacturerService.GetAll().ToList();
            man.ForEach(x => ManufacturersNamesList.Add(x.Name));

            var cat = service.CategoryService.GetAll().ToList();
            cat.ForEach(x => CategoriesNamesList.Add(x.Name));
        }


        public ICommand AddNewDrugCommand { get; set; }
        public ICommand ShowDrugDataCommand { get; set; }
        public ICommand OpenFileDialogCommand { get; set; }
        public ICommand InsertImageInSelectedCommand { get; set; }
        public ICommand DeleteDrugCommand { get; set; }
        public ICommand UpdateDrugCommand { get; set; }

        public ICommand ShowImage { get; set; }

        #endregion

        public ICommand BackCommand { get; set; }
    }
}
