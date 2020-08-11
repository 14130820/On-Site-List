using DustSwier.OnSiteList.Models;
using DustSwier.OnSiteList.Models.DataAccess;
using DustSwier.OnSiteList.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.ViewModels
{
    public class ManifestViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Manifest> manifests;

        public ManifestViewModel(BaseDataProvider dataProvider)
        {
            manifests = dataProvider.LoggingSystem.Manifests;

            AddCommand = new RelayCommand(AddExecuted);
            InCommand = new RelayCommand<Manifest>(InExecuted);
            OutCommand = new RelayCommand<Manifest>(OutExecuted);
            DeleteCommand = new RelayCommand<Manifest>(DeleteExecuted);
            EditCommand = new RelayCommand<Manifest>(EditExecuted);

        }
        

        public RelayCommand AddCommand { get; private set; }

        public RelayCommand<Manifest> InCommand { get; private set; }
        public RelayCommand<Manifest> OutCommand { get; private set; }

        public RelayCommand<Manifest> DeleteCommand { get; private set; }
        public RelayCommand<Manifest> EditCommand { get; private set; }

        public ObservableCollection<Manifest> Manifests => manifests;

        private void AddExecuted()
        {
            var message = SimpleIoc.Default.GetInstance<OpenManifestMessage>();
            message.SendData(null, false);
            

            if (message.HasData) Manifests.Add(message.Data);
        }

        private void InExecuted(Manifest guests)
        {
            new AddManifestMessage(guests, Status.IN);
        }
        private void OutExecuted(Manifest guests)
        {
            new AddManifestMessage(guests, Status.OUT);
        }

        private void DeleteExecuted(Manifest guests)
        {
            manifests.Remove(guests);
        }

        private void EditExecuted(Manifest guests)
        {
            var message = SimpleIoc.Default.GetInstance<OpenManifestMessage>();
            message.SendData(guests.Clone());

            if (message.HasData)
            {
                manifests[manifests.IndexOf(guests)] = message.Data;
            }
        }
    }
}
