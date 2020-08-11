using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace DustSwier.OnSiteList.Models.DataAccess
{
    /// <summary>
    /// loads the current file
    /// </summary>
    public class DesignTimeDataProvider : BaseDataProvider
    {
        public DesignTimeDataProvider() : base()
        {
            //for (int i = 0; i < 15; i++)
            //{
                //var room = new RoomID(i.GetChar(), 0, i * 2);
                //var name = "SURNAME, Name";

                //LoggingSystem.AddLog(room, Status.CHECKOUT);
                //loggingSystem.TrafficLog[i].Name = name;

                //var guest = new Room(room);
                //guest.CurrentGuestName = name;
                //guest.CurrentStatus = Status.CHECKOUT;
                //ObservableCollection<Room> guests = new ObservableCollection<Room>();
                //guests.Add(guest);
                //guests.Add(guest);
                //guests.Add(guest);
                //guests.Add(guest);
                //guests.Add(guest);

                //var MF = new Manifest();
                //MF.Title = name;
                //loggingSystem.Manifests.Add(MF);
            //}
        }

        protected override string Extention => ".json";

        protected override bool Save<T>(string filePath, T data)
        {
            return true;
        }

        protected override bool TryOpen<T>(string filePath, out T data)
        {
            data = default;
            return true;
        }
    }
}
