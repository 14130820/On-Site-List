using DustSwier.OnSiteList.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.Models
{
    public class LoggingSystemWrapper
    {
        public static LoggingSystemWrapper Instance { get; private set; }

        private readonly LoggingSystem loggingSystem;

        public LoggingSystemWrapper(LoggingSystem loggingSystem)
        {
            this.loggingSystem = loggingSystem;
            Instance = this;
        }

        public ObservableCollection<Log> UnlistedCheckins => loggingSystem.UnlistedCheckins;
        public ObservableCollection<Room> RoomCollection => loggingSystem.RoomDataStructure.RoomCollection;
        public ObservableCollection<Log> TrafficLog => loggingSystem.TrafficLog;
        public ObservableCollection<Manifest> Manifests => loggingSystem.Manifests;

        public delegate void StatusMismatch(RoomID room, Status oldStatus);
        public event StatusMismatch OnStatusMismatch;

        public bool TryGetRoom(RoomID roomID, out Room room) => loggingSystem.RoomDataStructure.TryGetRoom(roomID, out room);

        private Room CreateRoom(RoomID roomID) => loggingSystem.RoomDataStructure.CreateRoom(roomID);

        public void AddLog(RoomID roomID, Status status)
        {
            Log log = null;

            if (!TryGetRoom(roomID, out Room room))
            {
                room = CreateRoom(roomID);

                log = new Log(roomID, status, string.Empty, false);
            }
            else
            {            
                // If the new log status matches the old log, display warning
                var currentStatus = room.CurrentLog.Status;
                if (currentStatus == Status.INS && status == Status.INS ||
                    currentStatus == Status.OUTS && status == Status.OUTS)
                {
                    OnStatusMismatch?.Invoke(roomID, currentStatus);
                }

                log = new Log(roomID, status, room.CurrentLog.Name, room.CurrentLog.HasH2S);
            }

            room.AddLog(log);
            TrafficLog.Add(log);
        }

        public void RemoveLog(Log log)
        {
            var roomID = log.RoomID;

            if (TryGetRoom(roomID, out Room room))
            {
                room.RemoveLog(log);

                // If no logs.
                if (room.ActivityLogs.Count == 0)
                {
                    loggingSystem.RoomDataStructure.RemoveRoom(roomID);
                }
            }
        }
        
        /// <summary>
        /// Moves a log from one location to another.
        /// </summary>
        /// <param name="log">Log to move</param>
        /// <param name="roomID">Location to move to</param>
        public void ChangeLog(Log log, RoomID roomID)
        {
            RemoveLog(log);

            if (!TryGetRoom(roomID, out Room room))
            {
                room = CreateRoom(roomID);
            }
            else
            {           
                // If status the same
                var currentStatus = room.CurrentLog.Status;
                var status = log.Status;
                if (currentStatus == Status.INS && status == Status.INS ||
                    currentStatus == Status.OUTS && status == Status.OUTS)
                {
                    OnStatusMismatch?.Invoke(roomID, currentStatus);
                }
            }

            room.AddLog(log);
        }

        public void CleanUp() => loggingSystem.TrafficLog.Clear();
    }
}
