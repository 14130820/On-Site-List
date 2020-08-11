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
    public class RoomConverter : IValueConverter
    {
        /// <summary>
        /// Tries to parse a string into a Room Type.
        /// </summary>
        /// <param name="s">String to parse</param>
        /// <param name="r">Returns the room result</param>
        /// <returns>If the parse was successful.</returns>
        public static bool TryParse(string s, out RoomID r)
        {
            try
            {
                var stringCount = s.Length;
                if (stringCount >= 2 && stringCount <= 6)
                {
                    string[] digits = new string[stringCount];
                    for (int i = 0; i < stringCount; i++)
                    {
                        digits[i] = s.Substring(i, 1);
                    }

                    if (char.TryParse(digits[0], out char dorm) && char.IsLetter(dorm)) // First letter
                    {
                        dorm = char.ToLower(dorm);
                        int lengthToRoom = 0;
                        string secondDigit = digits[1];
                        int floor;
                        if (secondDigit == "-" && stringCount < 5) // B-...
                        {
                            floor = 0;
                            lengthToRoom = 2;
                        }
                        else if (int.TryParse(secondDigit, out floor)) // B0...
                        {
                            if (stringCount == 2)
                            {
                                floor = 0;
                                lengthToRoom = 1;
                            }
                            else
                            {
                                string thirdDigit = digits[2];
                                if (thirdDigit == "-" && stringCount < 6) // B0-...
                                {
                                    lengthToRoom = 3;
                                }
                                else if (int.TryParse(thirdDigit, out floor)) // B00...
                                {
                                    if (stringCount == 3)
                                    {
                                        floor = 0;
                                        lengthToRoom = 1;
                                    }
                                    else if (digits[3] == "-") // B00-...
                                    {
                                        lengthToRoom = 4;
                                    }
                                    else
                                    {
                                        throw new Exception("Invalid Input");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Invalid Input");
                                }
                            }

                        }
                        else
                        {
                            throw new Exception("Invalid Input");
                        }

                        // Solve last digits.
                        var roomDigits = stringCount - lengthToRoom;
                        if (!int.TryParse(s.Substring(lengthToRoom, roomDigits), out int room))
                        {
                            throw new Exception("Invalid Input");
                        }

                        // Fix floors
                        if (floor < 2) floor = 1;

                        r = new RoomID(dorm, (byte)floor, (byte)room);
                        return true;
                    }
                    else
                    {
                        throw new Exception("Invalid Input");
                    }
                }
            }
            catch { }
            r = new RoomID();
            return false;
        }

        public static RoomID Convert(string s)
        {
            TryParse(s, out RoomID room);
            return room;
        }

        /// <summary>
        /// Convert to string
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as RoomID).ToString();
        }

        /// <summary>
        /// Convert string to room
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TryParse(value as string, out RoomID room);
            return room;
        }
    }
}
