using DustSwier.OnSiteList.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DustSwier.OnSiteList.Converters
{
    public class StatusTypeConverter : IValueConverter
    {
        private readonly string[] stringStatus = new string[] { "CHECKIN", "IN", "OUT", "CHECKOUT" };

        public string[] StringStatus => stringStatus;

        /// <summary>
        /// Convert to string
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (Status)value;
            switch (status)
            {
                case Status.CHECKIN:
                    return 0;
                case Status.IN:
                    return 1;
                case Status.OUT:
                    return 2;
                case Status.CHECKOUT:
                    return 3;
            }
            throw new Exception("Can't convert object to a status string.");
        }

        /// <summary>
        /// Convert string to Status
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (int)value;
            switch (s)
            {
                case 0:
                    return Status.CHECKIN;
                case 1:
                    return Status.IN;
                case 2:
                    return Status.OUT;
                case 3:
                    return Status.CHECKOUT;
            }
            throw new Exception("Can't convert to status.");
        }
    }
}
