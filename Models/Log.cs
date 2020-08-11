using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DustSwier.OnSiteList.Models
{
    [JsonObject(IsReference = true)]
    public class Log : INotifyPropertyChanged
    {
        private readonly DateTime time;

        /// <summary>
        /// Raw room value
        /// </summary>
        [JsonIgnore]
        public RoomID roomID;

        private Status status;
        private string name;
        private bool hasH2S;

        public Log(RoomID roomID, Status status, string name, bool hasH2S)
        {
            this.roomID = roomID;
            this.status = status;
            this.name = name;
            this.hasH2S = hasH2S;
            this.time = DateTime.Now;
        }

        [JsonConstructor]
        public Log(Status status, string name, bool hasH2S, DateTime time)
        {
            this.status = status;
            this.name = name;
            this.hasH2S = hasH2S;
            this.time = time;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        public string CopyTimeFormat => time.ToString(format: @"HHmm");

        public DateTime Time => time;

        public Status Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        [JsonIgnore]
        public RoomID RoomID
        {
            get
            {
                return roomID;
            }
            set
            {
                if (!RoomID.Equals(value))
                {
                    LoggingSystemWrapper.Instance.ChangeLog(this, value);

                    roomID = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RoomID"));
                }
            }
        }

        public bool HasH2S
        {
            get
            {
                return hasH2S;
            }
            set
            {
                hasH2S = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasH2S"));
            }
        }
    }
}