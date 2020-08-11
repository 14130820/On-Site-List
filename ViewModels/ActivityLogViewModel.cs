using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using DustSwier.OnSiteList.Models;
using DustSwier.OnSiteList.ViewModels.Messages;

namespace DustSwier.OnSiteList.ViewModels
{
    public class ActivityLogViewModel : ViewModelBase
    {
        public ActivityLogViewModel(OpenActivityLogMessage message)
        {
            var guest = message.Data;
            ActivityLogs = guest.ActivityLogs;
            RoomText = string.Format("Room {0} Activity Logs.", guest.RoomID.ToString());
        }

        public ObservableCollection<Log> ActivityLogs { get; private set; }
        public string RoomText { get; private set; }
    }
}
