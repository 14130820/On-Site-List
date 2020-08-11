using DustSwier.OnSiteList.Models;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.ViewModels.Messages
{
    public class DeleteDisplayLogMessage : DefaultMessage<Log>
    {
        protected override void SendSelf()
        {
            Messenger.Default.Send(this);
        }
    }
}
