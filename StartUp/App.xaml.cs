using DustSwier.OnSiteList.Models.DataAccess;
using DustSwier.OnSiteList.ViewModels;
using DustSwier.OnSiteList.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DustSwier.OnSiteList
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                // Try save then throw error
                SimpleIoc.Default.GetInstance<BaseDataProvider>().SaveLogs();
                e.Handled = false;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SimpleIoc.Default.GetInstance<BaseDataProvider>().SaveLogs();
            base.OnExit(e);
        }
    }
}
