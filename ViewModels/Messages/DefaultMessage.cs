using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace DustSwier.OnSiteList.ViewModels.Messages
{
    public abstract class DefaultMessage<T>
    {
        /// <summary>
        /// Sends a message with T data.
        /// </summary>
        public void SendData(T data, bool hasData = true)
        {
            Data = data;
            HasData = hasData;
            SendSelf();
        }

        public T Data { get; set; }
        public bool HasData { get; set; }
        
        protected abstract void SendSelf();
    }

    public abstract class DefaultMessage
    {
        /// 
        /// <summary>
        /// Sends a message with no data.
        /// </summary>
        public void SendMessage()
        {
            SendSelf();
        }

        protected abstract void SendSelf();
    }
}
