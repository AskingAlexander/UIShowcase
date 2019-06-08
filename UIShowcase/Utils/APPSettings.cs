using System;
using Xamarin.Essentials;

namespace UIShowcase.Utils
{
    public static class APPSettings
    {
        public static string FavString
        {
            get => Preferences.Get("FAV_STRING", string.Empty);
            set => Preferences.Set("FAV_STRING", value);
        }

        public static string FavColor
        {
            get => Preferences.Get("FAV_COLOR", string.Empty);
            set => Preferences.Set("FAV_COLOR", value);
        }

        public static bool FavBool
        {
            get => Preferences.Get("FAV_BOOL", false);
            set => Preferences.Set("FAV_BOOL", value);
        }

        public static string DoubleToHex(this double givenValue)
        {
            byte[] bytes = BitConverter.GetBytes(givenValue);

            return BitConverter.ToString(bytes);
        }
    }
}
