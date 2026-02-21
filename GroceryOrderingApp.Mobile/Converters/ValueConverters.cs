using System.Globalization;

namespace GroceryOrderingApp.Mobile.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            var stringValue = value.ToString();
            return !string.IsNullOrWhiteSpace(stringValue);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CountToBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ICollection collection)
                return collection.Count > 0;
            return false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CountToInvertedBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ICollection collection)
                return collection.Count == 0;
            return true;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsPendingConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value?.ToString() == "Pending";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return !(bool)(value ?? false);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return !(bool)(value ?? false);
        }
    }

    // Alias for InverseBoolConverter used in some views
    public class InvertedBoolConverter : InverseBoolConverter { }

    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var status = value?.ToString() ?? string.Empty;
            
            return status switch
            {
                "Pending" => Color.FromArgb("#FFC107"), // Warning Orange
                "Delivered" => Color.FromArgb("#4CAF50"), // Success Green
                "Cancelled" => Color.FromArgb("#F44336"), // Error Red
                "Completed" => Color.FromArgb("#2196F3"), // Info Blue
                _ => Color.FromArgb("#757575") // Secondary Text
            };
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
