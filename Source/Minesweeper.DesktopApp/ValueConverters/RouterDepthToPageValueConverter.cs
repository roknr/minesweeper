using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Minesweeper.Core.Enums;
using Minesweeper.Core.Interfaces.Routing;
using Minesweeper.Core.Routing;
using Minesweeper.Core.ValueObjects;
using Minesweeper.Core.ViewModels.Pages;
using Minesweeper.DesktopApp.Pages;

namespace Minesweeper.DesktopApp.ValueConverters;

/// <summary>
/// The converter that converts a <see cref="uint"/> to an application page, derived from the <see cref="Page"/> class.
/// </summary>
public class RouterDepthToPageValueConverter : ValueConverterBase<RouterDepthToPageValueConverter>
{
    #region Private members

    /// <summary>
    /// The application router.
    /// </summary>
    /// <remarks>
    /// Ignore nullable error because it will always be set if not in design mode.
    /// </remarks>
    private readonly IRouter _router = null!;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="RouterDepthToPageValueConverter"/> class.
    /// </summary>
    public RouterDepthToPageValueConverter()
    {
#pragma warning disable RCS1208
        // Only set the router if not in design mode because IoC is not set up and a binding will not exist
        if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            _router = Core.IoC.Get<IRouter>();
        }
#pragma warning restore RCS1208
    }

    #endregion

    #region Converter methods

    /// <summary>
    /// Converts the selected <see cref="Route"/> on the router depth, specified by the converter input
    /// (the "value" parameter - a <see cref="uint"/>) to a page (derived from the <see cref="Page"/> class).
    /// </summary>
    /// <param name="value">The router depth (a <see cref="uint"/>).</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use. None when using this converter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
#if DEBUG
        // If in design mode, no IoC is set up and router won't exist, so just return a start page
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            return new StartPage();
        }
#endif

        // The value represents the router depth
        var depth = (uint)value;

        // Get the selected route from the router and convert it to a page
        var route = _router.GetActiveRoute(depth);

        return ConvertRouteToPage(route);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Only one way conversion is needed
        throw new NotImplementedException();
    }

    #endregion

    #region Private helpers

    /// <summary>
    /// Converts a <see cref="Route"/> to an application page derived from the <see cref="Page"/> class.
    /// </summary>
    /// <param name="route">The route to convert to an application page.</param>
    /// <returns></returns>
    private static Page ConvertRouteToPage(Route route)
    {
        // Create the page based on the page type
        switch (route.PageType)
        {
            case ApplicationPage.Start:
            {
                return CreateStartPage();
            }
            case ApplicationPage.Game:
            {
                // Argument is expected to be the game settings
                var gameSettings = (GameSettings)route.Argument!;

                return CreateGamePage(gameSettings);
            }
            default:
            {
                // Should never get to here
                throw new InvalidEnumArgumentException($"{nameof(route)}.{nameof(route.PageType)}", (int)route.PageType, typeof(ApplicationPage));
            }
        }
    }

    /// <summary>
    /// Creates and returns a <see cref="StartPage"/>.
    /// </summary>
    private static StartPage CreateStartPage()
    {
        // Create the start page and set its data context to a new start page view model
        return new StartPage
        {
            DataContext = new StartPageViewModel()
        };
    }

    /// <summary>
    /// Creates a <see cref="GamePage"/> with the specified game settings and returns it.
    /// </summary>
    /// <param name="gameSettings">The game settings.</param>
    private static GamePage CreateGamePage(GameSettings gameSettings)
    {
        // Create the game page and set its data context to a new game page view model with the
        // passed in game settings
        return new GamePage
        {
            DataContext = new GamePageViewModel(gameSettings)
        };
    }

    #endregion
}
