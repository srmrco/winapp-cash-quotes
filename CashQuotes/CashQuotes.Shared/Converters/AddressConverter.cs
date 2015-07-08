using System;
using Windows.UI.Xaml.Data;

namespace CashQuotes.Converters
{
	public class AddressConverter : IValueConverter
    {
	    public object Convert(object value, Type targetType, object parameter, string language)
	    {
		    var len = value.ToString().Length > 30 ? 30 : value.ToString().Length;
		    return value.ToString().Substring(0, len);
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, string language)
	    {
		    throw new NotImplementedException();
	    }
    }
}
