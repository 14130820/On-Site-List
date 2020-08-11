using Newtonsoft.Json;
using System.ComponentModel;

namespace DustSwier.OnSiteList.Models
{
    /// <summary>
    /// Holds a room ID and points to the real room ID values.
    /// </summary>
    public class ManifestItem : INotifyPropertyChanged
    {
        private RoomID roomID;

        public ManifestItem(RoomID room) => this.roomID = room;

        [JsonIgnore]
        public bool HasH2S
        {
            get
            {
                if (LoggingSystemWrapper.Instance.TryGetRoom(roomID, out Room room))
                {
                    return room.CurrentLog.HasH2S;
                }
                else
                {
                    return false;
                }
            }
        }

        [JsonIgnore]
        public string Name
        {
            get
            {
                if (LoggingSystemWrapper.Instance.TryGetRoom(roomID, out Room room))
                {
                    return room.CurrentLog.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public RoomID RoomID
        {
            get
            {
                return roomID;
            }
            set
            {
                roomID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasH2S"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
