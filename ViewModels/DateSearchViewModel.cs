using DustSwier.OnSiteList.Models;
using DustSwier.OnSiteList.Models.DataAccess;
using DustSwier.OnSiteList.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.ViewModels
{
    public class DateSearchViewModel : ViewModelBase
    {
        private readonly LoggingSystemWrapper loggingSystem;

        public DateSearchViewModel(BaseDataProvider dataProvider)
        {
            loggingSystem = dataProvider.LoggingSystem;

            SearchCommand = new RelayCommand(SearchExecuted);
            DeleteCommand = new RelayCommand<Log>(DeleteExecuted);

            var nowTime = DateTime.Now;
            FromDate = nowTime;
            ToDate = nowTime;

            MessengerInstance.Register<DeleteDisplayLogMessage>(this, DeleteLog);
        }

        private DateTime _toDate;
        private DateTime _fromDate;

        public DateTime FromDate
        {
            get => _fromDate;
            set {
                _fromDate = value;
                SearchCommand.RaiseCanExecuteChanged();
            }
        }
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                SearchCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Log> Logs { get; } = new ObservableCollection<Log>();

        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand<Log> DeleteCommand { get; private set; }
        
        private void SearchExecuted()
        {
            Logs.Clear();
            if (FromDate.CompareTo(ToDate) < 0)
            {
                var collection = loggingSystem.RoomCollection;
                var guestCount = collection.Count;
                for (int i = 0; i < guestCount; i++)
                {
                    var guest = collection[i];
                    var logs = guest.ActivityLogs;
                    var logCount = logs.Count;
                    for (int j = 0; j < logCount; j++)
                    {
                        var log = logs[j];
                        var logTime = log.Time;
                        if (logTime.CompareTo(FromDate) >= 0 &&
                            logTime.CompareTo(ToDate) <= 0)
                        {
                            Logs.Add(log);
                        }
                    }
                }
            }
        }

        private void DeleteExecuted(Log log) => new DeleteDisplayLogMessage().SendData(log);
        private void DeleteLog(DeleteDisplayLogMessage message) => Logs.Remove(message.Data);
    }
}
