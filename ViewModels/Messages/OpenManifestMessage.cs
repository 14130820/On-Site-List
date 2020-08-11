using DustSwier.OnSiteList.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.ViewModels.Messages
{
    public class OpenManifestMessage : DefaultMessage<Manifest>
    {
        protected override void SendSelf()
        {
            Messenger.Default.Send(this);
        }
    }
}
