using DustSwier.OnSiteList.Models;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.ViewModels.Messages
{
    public class CheckinMessage
    {
        public void SendData(RoomID roomID)
        {
            this.RoomID = roomID;
            Messenger.Default.Send(this);
        }

        public RoomID RoomID { get; set; }
        public string Name { get; set; }
        public bool HasH2S { get; set; }

        public bool CancelMessage { get; set; } = true;
    }
}
