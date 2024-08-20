using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Minesweeper.DesktopApp.ValueConverters;

/// <summary>
/// The base value converter implementation that is possible to use directly from XAML.
/// </summary>
/// <typeparam name="TValueConverter">The type of this value converter (the derived class).</typeparam>
public abstract class ValueConverterBase<TValueConverter> : MarkupExtension, IValueConverter
    where TValueConverter : class, new()
{
    /// <summary>
    /// The value converter instance that is being used.
    /// </summary>
    private static TValueConverter? ValueConverterInstance { get; set; } = null;

    /// <summary>
    /// Provides the instance of this value converter.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return ValueConverterInstance ??= new TValueConverter();
    }

    #region Value converter methods

    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

    #endregion
}
