using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DustSwier.OnSiteList.Models
{
    public class RoomDataStructure
    {
        private readonly ObservableCollection<Room> roomCollection = new ObservableCollection<Room>();
        private readonly Dictionary<byte, Dictionary<byte, Dictionary<byte, Room>>> roomDictionary;

        public RoomDataStructure() => roomDictionary = new Dictionary<byte, Dictionary<byte, Dictionary<byte, Room>>>();

        /// <summary>
        /// Creates the roomCollection in the start.
        /// </summary>
        [JsonConstructor()]
        public RoomDataStructure(Dictionary<byte, Dictionary<byte, Dictionary<byte, Room>>> roomDictionary)
        {
            this.roomDictionary = roomDictionary;

            // Create room Collection at start
            foreach (KeyValuePair<byte, Dictionary<byte, Dictionary<byte, Room>>> floorPair in roomDictionary)
            {
                var floor = floorPair.Value;
                foreach (KeyValuePair<byte, Dictionary<byte, Room>> dormPair in floor)
                {
                    var dorm = dormPair.Value;
                    foreach (KeyValuePair<byte, Room> roomPair in dorm)
                    {
                        roomCollection.Add(roomPair.Value);
                    }
                }
            }
        }

        [JsonIgnore]
        public ObservableCollection<Room> RoomCollection => roomCollection;

        public Dictionary<byte, Dictionary<byte, Dictionary<byte, Room>>> RoomDictionary => roomDictionary;

        public void RemoveRoom(RoomID roomID)
        {
            var floorID = roomID.Floor;
            var dormID = roomID.Dorm.ToByte();
            var roomNumber = roomID.RoomNumber;

            try
            {
                var floor = roomDictionary[floorID];
                var dorm = floor[dormID];

                if (dorm.Remove(roomNumber))
                {
                    if (dorm.Count == 0)
                    {
                        floor.Remove(dormID);

                        if (floor.Count == 0)
                        {
                            roomDictionary.Clear();
                        }
                    }
                }

                var length = roomCollection.Count;
                for (int i = 0; i < length; i++)
                {
                    if (roomCollection[i].RoomID.Equals(roomID))
                    {
                        roomCollection.RemoveAt(i);
                        break;
                    }
                }
            }
            catch { }
        }

        public bool TryGetRoom(RoomID roomID, out Room room)
        {
            try
            {
                room = roomDictionary[roomID.Floor][roomID.Dorm.ToByte()][roomID.RoomNumber];
                return true;
            }
            catch { }

            room = default;
            return false;
        }

        public Room CreateRoom(RoomID roomID)
        {
            // Get floor
            var floorID = roomID.Floor;
            if (!roomDictionary.TryGetValue(floorID, out Dictionary<byte, Dictionary<byte, Room>> floor))
            {
                floor = new Dictionary<byte, Dictionary<byte, Room>>();
                roomDictionary.Add(floorID, floor);
            }

            // Get Dorm
            var dormID = roomID.Dorm.ToByte();
            if (!floor.TryGetValue(dormID, out Dictionary<byte, Room> dorm))
            {
                dorm = new Dictionary<byte, Room>();
                floor.Add(dormID, dorm);
            }

            // Get Room
            var roomNumber = roomID.RoomNumber;
            if (!dorm.TryGetValue(roomNumber, out Room room))
            {
                room = new Room(roomID);

                dorm.Add(roomNumber, room);
                roomCollection.Add(room);
            }

            return room;
        }
    }
}
