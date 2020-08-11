using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DustSwier.OnSiteList.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            MainGrid.SelectionChanged += DataGrid_SelectionChanged;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = e.AddedItems;
            if (selectedItems.Count > 0)
            {
                MainGrid.ScrollIntoView(selectedItems[0]);
            }
        }

        /// <summary>
        /// For copy all
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender == CopyAll)
            {
                var grid = MainGrid;
                grid.SelectAll();
                ApplicationCommands.Copy.Execute(null, grid);
                grid.SelectedIndex = -1;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator locator = (ViewModelLocator)FindResource("Locator");
            locator.OpenGuestExecuted();
        }

        /// <summary>
        /// Removes unnecessary columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainGrid_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            e.ClipboardRowContent.RemoveAt(0);
            e.ClipboardRowContent.RemoveAt(2);
        }
        
        private void RoomInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            roomInputWaterMark.Visibility = string.IsNullOrEmpty(roomInput.Text) ? Visibility.Visible : Visibility.Hidden;
        }

        ///// <summary>
        ///// Force focused controls to lose focus on click.
        ///// This is used to force the textbox to lose focus for updates.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Menu_MouseClick(object sender, MouseButtonEventArgs e)
        //{
        //    IInputElement focusedControl = FocusManager.GetFocusedElement(this);
        //    if (focusedControl != null)
        //        focusedControl.RaiseEvent(new RoutedEventArgs(LostFocusEvent));
        //}
    }
}
