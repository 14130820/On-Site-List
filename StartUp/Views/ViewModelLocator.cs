using CommonServiceLocator;
using DustSwier.OnSiteList.Models;
using DustSwier.OnSiteList.ViewModels;
using DustSwier.OnSiteList.ViewModels.Messages;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DustSwier.OnSiteList.Views
{
    /// <summary>
    /// For wiring up views and View Models
    /// Also works as a bootstrapper in constructor.
    /// </summary>
    public class ViewModelLocator : ViewModelBase
    {
        private readonly SimpleIoc ioc = SimpleIoc.Default;
        private readonly IMessenger messenger = Messenger.Default;

        public ViewModelLocator()
        {
            Bootstrapper boot = new Bootstrapper();

            messenger.Register<OpenActivityLogMessage>(this, OpenActivityLogView);
            messenger.Register<OpenManifestMessage>(this, OpenManifestMessage);
            messenger.Register<OpenInfoMessage>(this, OpenInfoMessage);
            messenger.Register<CheckinMessage>(this, OpenCheckinMessage);

            OpenGuestCommand = new RelayCommand(OpenGuestExecuted);
            OpenDateSearchCommand = new RelayCommand(OpenDateSearchExecuted);
            OpenManifestCommand = new RelayCommand(OpenManifestExecuted);
            OpenCheckinsCommand = new RelayCommand(OpenCheckinsExecuted);
    }

        public RelayCommand OpenGuestCommand { get; private set; }
        public RelayCommand OpenDateSearchCommand { get; private set; }
        public RelayCommand OpenManifestCommand { get; private set; }
        public RelayCommand OpenCheckinsCommand { get; private set; }

        #region Messages

        private void OpenDateSearchExecuted() => SideContent = ioc.GetInstance<DateSearchView>();

        public void OpenGuestExecuted() => SideContent = ioc.GetInstance<GuestView>();

        public void OpenManifestExecuted() => SideContent = ioc.GetInstance<ManifestView>();

        public void OpenCheckinsExecuted() => SideContent = ioc.GetInstance<CheckinsView>();

        private void OpenCheckinMessage(CheckinMessage message)
        {
            var view = ioc.GetInstanceWithoutCaching<SetCheckinView>();
            view.Owner = Application.Current.MainWindow;
            view.ShowDialog();
        }

        private void OpenManifestMessage(OpenManifestMessage message)
        {
            var view = ioc.GetInstanceWithoutCaching<AddManifestView>();
            view.Owner = Application.Current.MainWindow;
            view.ShowDialog();
        }

        private void OpenActivityLogView(OpenActivityLogMessage message)
        {
            var view = ioc.GetInstanceWithoutCaching<ActivityLogView>();
            view.Owner = Application.Current.MainWindow;
            view.ShowDialog();
        }

        private void OpenInfoMessage(OpenInfoMessage message)
        {
            OpenUnclosableView<InfoView>();
        }

        /// <summary>
        /// Opens a non-closing view.
        /// View close must override to Hide();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OpenUnclosableView<T>()
        {
            var view = ioc.GetInstance<T>() as Window;

            if (view.IsVisible)
            {
                view.Activate();
            }
            else
            {
                view.Show();
            }
        }

        #endregion


        #region Binding Properties

        public MainViewModel MainViewModel => ioc.GetInstance<MainViewModel>();
        public GuestViewModel GuestViewModel => ioc.GetInstance<GuestViewModel>();
        public ManifestViewModel ManifestViewModel => ioc.GetInstance<ManifestViewModel>();
        public ActivityLogViewModel ActivityLogViewModel => ioc.GetInstanceWithoutCaching<ActivityLogViewModel>();
        public DateSearchViewModel DateSearchViewModel => ioc.GetInstance<DateSearchViewModel>();
        public SetCheckinViewModel SetCheckinViewModel => ioc.GetInstanceWithoutCaching<SetCheckinViewModel>();

        public AddManifestViewModel CurrentAddManifestViewModel { get; private set; }
        public AddManifestViewModel AddManifestViewModel
        {
            get
            {
                var view = ioc.GetInstanceWithoutCaching<AddManifestViewModel>();
                CurrentAddManifestViewModel = view;
                return view;
            }
        }

        private Grid sideContent;
        public Grid SideContent
        {
            get => sideContent;
            set
            {
                sideContent = value;
                RaisePropertyChanged();
            }
        }

        #endregion
        
    }
}