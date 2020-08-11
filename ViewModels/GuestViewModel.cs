using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using DustSwier.OnSiteList.Models;
using DustSwier.OnSiteList.ViewModels.Messages;
using DustSwier.OnSiteList.Models.DataAccess;
using GalaSoft.MvvmLight.Ioc;

namespace DustSwier.OnSiteList.ViewModels
{
    public class GuestViewModel : ViewModelBase
    {
        public GuestViewModel(BaseDataProvider dataProvider)
        {
            var guestList = dataProvider.LoggingSystem.RoomCollection;
            Logs = guestList;

            ShowActivityCommand = new RelayCommand<Room>(ShowActivityExecuted);

            MessengerInstance.Register<SelectedGuestMessage>(this, UpdateSelection);
        }

        private void UpdateSelection(SelectedGuestMessage message)
        {
            var room = message.Data;
            var logCount = Logs.Count;
            for (int i = 0; i < logCount; i++)
            {
                if (Logs[i].RoomID.Equals(room))
                {
                    SelectedItem = Logs[i];
                    RaisePropertyChanged("SelectedItem");
                    break;
                }
            }
        }
        
        public ObservableCollection<Room> Logs { get; private set; }
        public Room SelectedItem { get; set; }


        public RelayCommand<Room> ShowActivityCommand { get; private set; }

        private void ShowActivityExecuted(Room guest)
        {
            SimpleIoc.Default.GetInstance<OpenActivityLogMessage>().SendData(guest);
        }
    }
}
