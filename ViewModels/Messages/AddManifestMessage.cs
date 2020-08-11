using DustSwier.OnSiteList.Models;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.ViewModels.Messages
{
    public class AddManifestMessage
    {
        private readonly Manifest manifest;
        private readonly Status status;

        public AddManifestMessage(Manifest manifest, Status status)
        {
            this.manifest = manifest;
            this.status = status;
            Messenger.Default.Send(this);
        }

        public Manifest Manifest => manifest;

        public Status Status => status;
    }
}
