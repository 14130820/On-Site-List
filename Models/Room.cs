using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;

namespace DustSwier.OnSiteList.Models
{
    [JsonObject(IsReference = true)]
    public class Room : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Log> activityLogs;
        private readonly RoomID roomID;

        private Log currentLog;

        public event PropertyChangedEventHandler PropertyChanged;

        public Room(RoomID roomID)
        {
            activityLogs = new ObservableCollection<Log>();
            this.roomID = roomID;
        }

        [JsonConstructor]
        public Room(RoomID roomID, ObservableCollection<Log> activityLogs)
        {
            this.roomID = roomID;
            this.activityLogs = activityLogs;
            CurrentLog = activityLogs[activityLogs.Count - 1];

            foreach (var item in activityLogs)
            {
                item.roomID = this.roomID;
            }
        }

        /// <summary>
        /// Adds a log to the room making sure that the logs are organized by time.
        /// </summary>
        public void AddLog(Log log)
        {
            //log.PropertyChanged += LogPropertyChanged;

            var time = log.Time;
            var logCount = activityLogs.Count;

            // Inserts the log into the index ordered by time.
            for (int i = logCount - 1; i >= 0; i--)
            {
                var actLogTime = activityLogs[i].Time;

                if (time.CompareTo(actLogTime) >= 0)
                {
                    activityLogs.Insert(i + 1, log);

                    if (i + 1 == logCount)
                    {
                        CurrentLog = activityLogs[i + 1];
                    }

                    return;
                }
            }

            activityLogs.Add(log);
            CurrentLog = log;
        }

        public void RemoveLog(Log log)
        {
            //log.PropertyChanged -= LogPropertyChanged;

            var logCount = activityLogs.Count;
            var index = activityLogs.IndexOf(log);

            if (index == logCount - 1 && logCount - 2 >= 0)
            {
                CurrentLog = activityLogs[logCount - 2];
            }

            ActivityLogs.RemoveAt(index);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActivityLogs"));
        }

        public RoomID RoomID => roomID;
        public ObservableCollection<Log> ActivityLogs => activityLogs;

        [JsonIgnore]
        public Log CurrentLog
        {
            get => currentLog;
            private set
            {
                currentLog = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentLog"));
            }
        }

        [JsonIgnore]
        public string RoomDisplay => roomID.ToString();
    }
}
