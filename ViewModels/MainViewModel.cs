using System;
using GalaSoft.MvvmLight;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using DustSwier.OnSiteList.Models.DataAccess;
using DustSwier.OnSiteList.Models;
using DustSwier.OnSiteList.ViewModels.Messages;
using DustSwier.OnSiteList.Converters;
using System.Collections;
using MonitoredUndo;
using GalaSoft.MvvmLight.Ioc;

namespace DustSwier.OnSiteList.ViewModels
{
    public class MainViewModel : ViewModelBase, ISupportsUndo
    {
        public MainViewModel(BaseDataProvider dataProvider)
        {
            InitializeCommandBindings();
            this.dataProvider = dataProvider;
            ChangeTrafficLog();

            MessengerInstance.Register<AddManifestMessage>(this, AddManifest);
            MessengerInstance.Register<DeleteDisplayLogMessage>(this, DeleteMessage);

            LoggingSystem.OnStatusMismatch += LoggingSystem_OnStatusMismatch;
            // link all collections.
            //Subscribe to changes from loaded data
            var guestCollection = LoggingSystem.RoomCollection;
            var manifestCollection = LoggingSystem.Manifests;
            var newGuestCollection = LoggingSystem.UnlistedCheckins;

            TrafficLog.CollectionChanged += Collection_CollectionChanged;
            guestCollection.CollectionChanged += Collection_CollectionChanged;
            manifestCollection.CollectionChanged += CollectionChanged;
            newGuestCollection.CollectionChanged += Collection_CollectionChanged;


            //Subscribe to changes from loaded data
            var length = TrafficLog.Count;
            for (int i = 0; i < length; i++)
            {
                TrafficLog[i].PropertyChanged += Model_PropertyChanged;
            }

            length = guestCollection.Count;
            for (int i = 0; i < length; i++)
            {
                guestCollection[i].PropertyChanged += Model_PropertyChanged;
            }
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SaveRequired = true;
        }

        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                var items = e.OldItems;
                var length = items.Count;
                for (int i = 0; i < length; i++)
                {
                    var item = items[i] as INotifyPropertyChanged;
                    item.PropertyChanged -= Model_PropertyChanged;
                }
            }
            else if (e.NewItems != null)
            {
                var items = e.NewItems;
                var length = items.Count;
                for (int i = 0; i < length; i++)
                {
                    var item = items[i] as INotifyPropertyChanged;
                    item.PropertyChanged += Model_PropertyChanged;
                }
            }

            SaveRequired = true;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveRequired = true;
        }

        private void LoggingSystem_OnStatusMismatch(RoomID room, Status oldStatus)
        {
            switch (oldStatus)
            {
                case Status.INS:
                    InfoText = string.Format("Warning: {0} was already in.", room.ToString());
                    break;
                case Status.OUTS:
                    InfoText = string.Format("Warning: {0} was already out.", room.ToString());
                    break;
            }
        }

        private void DeleteMessage(DeleteDisplayLogMessage message)
        {
            var log = message.Data;

            TrafficLog.Remove(log);
            LoggingSystem.RemoveLog(log);
            LoggingSystem.UnlistedCheckins.Remove(log);
        }

        private void AddManifest(AddManifestMessage message)
        {
            var status = message.Status;
            var logs = message.Manifest.Items;

            InfoText = string.Empty;
            var count = logs.Count;

            for (int i = 0; i < count; i++)
            {
                LoggingSystem.AddLog(logs[i].RoomID, status);
            }

            SelectedLog = TrafficLog.Count - 1;
            SaveRequired = true;

        }

        // Private fields
        private readonly BaseDataProvider dataProvider;

        private string userRoomInput;
        private string infoText;
        private bool saveRequired = false;

        // Private Properties
        private LoggingSystemWrapper LoggingSystem => dataProvider.LoggingSystem;


        #region Binding Properties

        public ObservableCollection<Log> TrafficLog => LoggingSystem.TrafficLog;

        public string LogDate { get; private set; }

        private int selectedLog;
        public int SelectedLog
        {
            get => selectedLog;
            set
            {
                selectedLog = value;
                RaisePropertyChanged();
            }
        }

        private object selectedValue;
        public object SelectedValue
        {
            get
            {
                return selectedValue;
            }
            set
            {
                selectedValue = value;
                DelCommand.RaiseCanExecuteChanged();
            }
        }

        public string UserRoomInput
        {
            get
            {
                return userRoomInput;
            }
            set
            {
                userRoomInput = value;

                isValidRoom = RoomConverter.TryParse(UserRoomInput, out cachedRoom);

                if (isValidRoom) new SelectedGuestMessage().SendData(cachedRoom);

                CheckInCommand.RaiseCanExecuteChanged();
                InCommand.RaiseCanExecuteChanged();
                OutCommand.RaiseCanExecuteChanged();
                CheckOutCommand.RaiseCanExecuteChanged();
            }
        }

        public string InfoText
        {
            get
            {
                return infoText;
            }
            set
            {
                infoText = value;
                RaisePropertyChanged();
            }
        }

        #endregion


        #region Commands

        public RelayCommand NewCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand UndoCommand { get; private set; }
        public RelayCommand RedoCommand { get; private set; }

        public RelayCommand InCommand { get; private set; }
        public RelayCommand OutCommand { get; private set; }
        public RelayCommand CheckInCommand { get; private set; }
        public RelayCommand CheckOutCommand { get; private set; }

        public RelayCommand<Log> DelCommand { get; private set; }

        public RelayCommand InfoCommand { get; private set; }

        private void InitializeCommandBindings()
        {
            //SaveCommand = new RelayCommand(SaveExecuted, () => SaveCanExecute()); test
            NewCommand = new RelayCommand(NewExecuted);
            SaveCommand = new RelayCommand(SaveExecuted, () => IsSaveRequired);

            UndoCommand = new RelayCommand(UndoExecuted, () => UndoCanExecute);
            RedoCommand = new RelayCommand(RedoExecuted, () => RedoCanExecute);

            CheckInCommand = new RelayCommand(CheckInExecuted, () => isValidRoom);
            InCommand = new RelayCommand(InExecuted, () => isValidRoom);
            OutCommand = new RelayCommand(OutExecuted, () => isValidRoom);
            CheckOutCommand = new RelayCommand(CheckOutExecuted, () => isValidRoom);

            DelCommand = new RelayCommand<Log>((log) => new DeleteDisplayLogMessage().SendData(log));

            InfoCommand = new RelayCommand(InfoExecuted);
        }

        #endregion


        #region Command Execution

        private bool UndoCanExecute => UndoService.Current[this].CanUndo;
        private bool RedoCanExecute => UndoService.Current[this].CanRedo;
        private void UndoExecuted() => UndoService.Current[this].Undo();
        private void RedoExecuted() => UndoService.Current[this].Redo();

        private void InfoExecuted() => new OpenInfoMessage().SendMessage();

        private void NewExecuted()
        {
            //clear loggless rooms & clear logged logs
            LoggingSystem.CleanUp();
            ChangeTrafficLog();
        }

        private void SaveExecuted()
        {
            if (dataProvider.SaveLogs())
            {
                InfoText = string.Empty;
                SaveRequired = false;
            }
            else
            {
                InfoText = "ERROR: Save unsuccessful";
            }
        }

        private void CheckOutExecuted() => AddLog(Status.CHECKOUT);
        private void OutExecuted() => AddLog(Status.OUT);
        private void InExecuted() => AddLog(Status.IN);

        private void CheckInExecuted()
        {
            var message = SimpleIoc.Default.GetInstanceWithoutCaching<CheckinMessage>();
            message.SendData(cachedRoom);

            if (!message.CancelMessage)
            {
                AddLog(Status.CHECKIN);
            }
        }

        /// <summary>
        /// The type Room inputted by the user.
        /// </summary>
        private RoomID cachedRoom;
        private bool isValidRoom;

        private bool IsSaveRequired => SaveRequired;

        public bool SaveRequired
        {
            get => saveRequired;
            private set
            {
                saveRequired = value;
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion


        #region Inlining

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddLog(Status status)
        {
            InfoText = string.Empty;

            LoggingSystem.AddLog(cachedRoom, status);

            SaveRequired = true;

            userRoomInput = string.Empty;
            RaisePropertyChanged("UserRoomInput");

            SelectedLog = TrafficLog.Count - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ChangeTrafficLog()
        {
            const string dateFormat = @"MMM dd HH:mm";

            LogDate = string.Format("Log Created on: {0}", DateTime.Now.ToString(dateFormat));
            RaisePropertyChanged("LogDate");
            RaisePropertyChanged("TrafficLog");

            GC.Collect();
        }

        #endregion

        public object GetUndoRoot() => this;
    }
}
