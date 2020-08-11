using CommonServiceLocator;
using DustSwier.OnSiteList.Models.DataAccess;
using DustSwier.OnSiteList.ViewModels;
using DustSwier.OnSiteList.ViewModels.Messages;
using DustSwier.OnSiteList.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System.Runtime.CompilerServices;

namespace DustSwier.OnSiteList
{
    public class Bootstrapper
    {
        public Bootstrapper()
        {
            var ioc = SimpleIoc.Default;
            ServiceLocator.SetLocatorProvider(new ServiceLocatorProvider(() => ioc));

            // Create design time view services and models
            if (ViewModelBase.IsInDesignModeStatic)
            {
                ioc.Register<BaseDataProvider, DesignTimeDataProvider>();
            }
            // Create run time view services and models
            else
            {
                ioc.Register<BaseDataProvider, JSONdataProvider>();
            }

            RegisterTypes(ioc);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RegisterTypes(SimpleIoc ioc)
        {
            // V/VM
            ioc.Register<ViewModelLocator>();

            ioc.Register<MainView>();
            ioc.Register<MainViewModel>();

            ioc.Register<GuestViewModel>();
            ioc.Register<GuestView>();

            ioc.Register<ManifestViewModel>();
            ioc.Register<ManifestView>();

            ioc.Register<ActivityLogViewModel>();
            ioc.Register<ActivityLogView>();

            ioc.Register<InfoView>();

            ioc.Register<DateSearchView>();
            ioc.Register<DateSearchViewModel>();

            ioc.Register<AddManifestView>();
            ioc.Register<AddManifestViewModel>();

            ioc.Register<CheckinsView>();
            ioc.Register<CheckinsViewModel>();

            ioc.Register<SetCheckinView>();
            ioc.Register<SetCheckinViewModel>();

            // Factories
            ioc.Register<OpenActivityLogMessage>();
            ioc.Register<OpenManifestMessage>();
            ioc.Register<CheckinMessage>();
        }
    }
}
