namespace WixWPFWizardBA.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class EnumToVisibilityConverter : IValueConverter
    {
        public EnumToVisibilityConverter()
        {
            this.TrueValue = Visibility.Visible;
            this.FalseValue = Visibility.Collapsed;
        }

        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter) ? this.TrueValue : this.FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            {
                if (Equals(value, this.TrueValue))
                    return parameter;
                if (Equals(value, this.FalseValue))
                    return Binding.DoNothing;
                return null;
            }
        }
    }
}