using System;
using System.Globalization;
using Minesweeper.Core.Enums;

namespace Minesweeper.DesktopApp.ValueConverters
{
    /// <summary>
    /// The converter that converts an <see cref="ApplicationTheme"/> to a font awesome icon.
    /// </summary>
    public class ApplicationThemeToIconValueConverter : ValueConverterBase<ApplicationThemeToIconValueConverter>
    {
        /// <summary>
        /// The application light theme icon.
        /// </summary>
        private const string LightThemeIcon = "\uf186";

        /// <summary>
        /// The application dark theme icon.
        /// </summary>
        private const string DarkThemeIcon = "\uf185";

        /// <summary>
        /// Converts the application theme to a font awesome icon.
        /// </summary>
        /// <param name="value">The application theme (a <see cref="ApplicationTheme"/>).</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use. None when using this converter.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var theme = (ApplicationTheme)value;

            switch (theme)
            {
                case ApplicationTheme.Light:
                    {
                        return LightThemeIcon;
                    }
                case ApplicationTheme.Dark:
                    {
                        return DarkThemeIcon;
                    }
                default:
                    {
                        // Should never get to here
                        return null!;
                    }
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Only one way conversion is needed
            throw new NotImplementedException();
        }
    }
}
