using UIShowcase.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UIShowcase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritePicker : ContentPage
    {
        public FavoritePicker()
        {
            InitializeComponent();
        }

        private void ColorPicker_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            string hex = string.Format(
                "#{0:X2}{1:X2}{2:X2}{3:X2}",
                255,
                (int)RedPicker.Value,
                (int)GreenPicker.Value,
                (int)BluePicker.Value);
            Color newColor = Color.FromHex(hex);

            ColorViewer.ProgressColor = newColor;
        }

        protected override bool OnBackButtonPressed()
        {
            if (string.IsNullOrEmpty(FavString.Text))
            {
                return false;
            }

            string hex = string.Format(
                "#{0:X2}{1:X2}{2:X2}{3:X2}",
                255,
                (int)RedPicker.Value,
                (int)GreenPicker.Value,
                (int)BluePicker.Value);
            APPSettings.FavColor = hex;
            APPSettings.FavBool = FavBool.IsToggled;
            APPSettings.FavString = FavString.Text;

            return base.OnBackButtonPressed();
        }
    }
}