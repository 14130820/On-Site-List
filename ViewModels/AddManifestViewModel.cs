using DustSwier.OnSiteList.Converters;
using DustSwier.OnSiteList.Models;
using DustSwier.OnSiteList.Models.DataAccess;
using DustSwier.OnSiteList.ViewModels.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.ViewModels
{
    public class AddManifestViewModel : ViewModelBase
    {
        private readonly OpenManifestMessage saveToManifest;
        private readonly Manifest manifest;

        public AddManifestViewModel(OpenManifestMessage openManifestMessage)
        {
            saveToManifest = openManifestMessage;

            manifest = openManifestMessage.HasData ? openManifestMessage.Data : new Manifest();

            AddCommand = new RelayCommand(AddExecuted, () => CanAdd);
            SaveCommand = new RelayCommand(SaveExecuted, () => Manifests.Count > 0 && !string.IsNullOrEmpty(manifest.Title));
            DeleteCommand = new RelayCommand<ManifestItem>(DeleteExecuted);

            openManifestMessage.HasData = false;
        }

        public ObservableCollection<ManifestItem> Manifests => manifest.Items;
        public string Title
        {
            get => manifest.Title;
            set
            {
                manifest.Title = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string Room
        {
            get => _room;
            set
            {
                _room = value;
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand<ManifestItem> DeleteCommand { get; private set; }

        private RoomID cachedRoom;
        private string _room;

        private bool CanAdd => RoomConverter.TryParse(Room, out cachedRoom);

        private void AddExecuted()
        {
            var item = new ManifestItem(cachedRoom);

            Manifests.Add(item);

            Room = string.Empty;
            RaisePropertyChanged("Room");

            SaveCommand.RaiseCanExecuteChanged();
        }

        private void SaveExecuted()
        {
            saveToManifest.Data = manifest;
            saveToManifest.HasData = true;
        }

        private void DeleteExecuted(ManifestItem manifestItem)
        {
            manifest.Items.Remove(manifestItem);

            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
