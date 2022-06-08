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
using System.Windows.Media.Imaging;

namespace Project.UI.ViewModels
{
    public class BuyDrugViewModel : BaseNotifyPropertyChanged
    {
        private ServiceStorage service;
        private UserViewModel userViewModel;
        private UserDTO currentUser;
        private DrugDTO currentDrug;

        public int CountToBuy { get; set; }
        public DrugDTO CurrentDrug
        {
            get => currentDrug;
            set
            {
                if(currentDrug != value)
                {
                    currentDrug = value;
                    NotifyOfPropertyChanged();
                }
            }
        }

        public BitmapImage Image { get; set; }

        public BuyDrugViewModel(ServiceStorage service, DrugDTO currentDrug, UserDTO currentUser, UserViewModel userViewModel)
        {
            this.service = service;
            this.userViewModel = userViewModel;
            this.currentUser = currentUser;
            CurrentDrug = currentDrug;
            Image = ByteArrayConverter.GetBitmapImage(CurrentDrug.Image);

            InitCommands();

        }

        private void InitCommands()
        {
            BuyCommand = new RelayCommand(x =>
            {
                try
                {
                    if (currentDrug.DrugCount - CountToBuy >= 0)
                    {
                        if (MessageBox.Show($"Sum is {CurrentDrug.DrugPrice * CountToBuy}\nAre You Sure", "Buying Drugs", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            //new EmailSender(currentUser).SendInfoAfterBuyingDrug();
                            MessageBox.Show("Succes!\nInformation about your drugs have been sent on your Email");
                            CurrentDrug.DrugCount -= CountToBuy;
                            service.DrugService.UpdateAsync(CurrentDrug);
                        }
                    }
                    else
                        MessageBox.Show("You cannot buy more than we have");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong :(");
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    CurrentDrug = service.DrugService.Get(CurrentDrug.Id);
                    RefreshInfo.Execute(currentDrug);
                }
            });
            RefreshInfo = new RelayCommand(x =>
            {
                CurrentDrug = service.DrugService.Get(CurrentDrug.Id);

            });
        }

        public ICommand BuyCommand { get; set; }
        public ICommand RefreshInfo { get; set; }
    }
}
