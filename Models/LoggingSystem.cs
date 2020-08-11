using DustSwier.OnSiteList.Models.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.Models
{
    public class LoggingSystem
    {
        private readonly RoomDataStructure roomDataStructure;
        private readonly ObservableCollection<Manifest> manifests;
        private readonly ObservableCollection<Log> unlistedCheckins;

        public LoggingSystem()
        {
            roomDataStructure = new RoomDataStructure();
            manifests = new ObservableCollection<Manifest>();
            TrafficLog = new ObservableCollection<Log>();
            unlistedCheckins = new ObservableCollection<Log>();
        }

        [JsonConstructor]
        public LoggingSystem(RoomDataStructure roomDataStructure, ObservableCollection<Manifest> manifests, ObservableCollection<Log> unlistedCheckins)
        {
            this.unlistedCheckins = unlistedCheckins;
            this.roomDataStructure = roomDataStructure;
            this.manifests = manifests;
        }

        public RoomDataStructure RoomDataStructure => roomDataStructure;
        
        public ObservableCollection<Log> TrafficLog { get; set; }
        public ObservableCollection<Manifest> Manifests => manifests;
        public ObservableCollection<Log> UnlistedCheckins => unlistedCheckins;
    }
}
