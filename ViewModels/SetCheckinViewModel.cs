using DustSwier.OnSiteList.Converters;
using DustSwier.OnSiteList.Models;
using DustSwier.OnSiteList.Models.DataAccess;
using DustSwier.OnSiteList.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.ViewModels
{
    public class SetCheckinViewModel : ViewModelBase
    {
        private readonly CheckinMessage message;

        public SetCheckinViewModel(CheckinMessage message)
        {
            this.message = message;

            AddCommand = new RelayCommand(AddExecuted);
        }

        public RelayCommand AddCommand { get; private set; }

        public string Name
        {
            get => message.Name;
            set => message.Name = value;
        }

        public RoomID Room
        {
            get => message.RoomID;
            set => message.RoomID = value;
        }

        public bool HasH2S
        {
            get => message.HasH2S;
            set => message.HasH2S = value;
        }

        private void AddExecuted() => message.CancelMessage = false;
    }
}
