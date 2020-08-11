using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using DustSwier.OnSiteList.Models;
using DustSwier.OnSiteList.ViewModels.Messages;
using DustSwier.OnSiteList.Models.DataAccess;
using System;
using GalaSoft.MvvmLight.Command;

namespace DustSwier.OnSiteList.ViewModels
{
    public class CheckinsViewModel : ViewModelBase
    {
        public CheckinsViewModel(BaseDataProvider dataProvider)
        {
            Logs = dataProvider.LoggingSystem.UnlistedCheckins;

            DeleteCommand = new RelayCommand<Log>(DeleteExecuted);
        }

        public ObservableCollection<Log> Logs { get; }

        public RelayCommand<Log> DeleteCommand { get; private set; }

        private void DeleteExecuted(Log log) => new DeleteDisplayLogMessage().SendData(log);
    }
}