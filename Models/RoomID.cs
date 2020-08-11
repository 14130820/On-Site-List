using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace DustSwier.OnSiteList.Models
{
    /// <summary>
    /// Represents a room using a dorm letter, floor number, and room number.
    /// </summary>
    public class RoomID
    {
        private readonly char _dorm;
        private readonly byte _floor;
        private readonly byte _roomNumber;
        private readonly string _toString;

        public char Dorm => _dorm;
        public byte Floor => _floor;
        public byte RoomNumber => _roomNumber;

        [JsonConstructor]
        public RoomID(char dorm, byte floor = 0, byte roomNumber = 0)
        {
            _floor = floor;
            _roomNumber = roomNumber;
            _dorm = dorm;
            _toString = string.Format("{0}{1}-{2:00}", char.ToUpper(_dorm), floor < 2 ? string.Empty : _floor.ToString(), _roomNumber);
        }

        public RoomID()
        {
            _toString = string.Empty;
        }

        /// <summary>
        /// Returns a cached string using {dorm}{floor}-{room number}.
        /// </summary>
        public override string ToString()
        {
            return _toString;
        }

        /// <summary>
        /// Warning: Only compares room number.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is RoomID room &&
                   _floor == room._floor &&
                   _roomNumber == room._roomNumber &&
                   _dorm == room.Dorm;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
