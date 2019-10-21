using System;
using System.Globalization;
using System.Windows.Controls;
using Minesweeper.Core.Enums;
using Minesweeper.Core.ValueObjects;
using Minesweeper.Core.ViewModels.Pages;
using Minesweeper.DesktopApp.Pages;

namespace Minesweeper.DesktopApp.ValueConverters
{
    /// <summary>
    /// The converter that converts an <see cref="ApplicationPage"/> to an actual page,
    /// that is derived from the <see cref="Page"/> class.
    /// </summary>
    public class ApplicationPageToPageValueConverter : ValueConverterBase<ApplicationPageToPageValueConverter>
    {
        #region Converter methods

        /// <summary>
        /// Converts a <see cref="ApplicationPage"/> to an actual page.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // The value represents the application page type
            var pageType = (ApplicationPage)value;

            // Based on the page type, create the corresponding page
            return ConvertPage(pageType, parameter);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Only one way conversion is needed
            throw new NotImplementedException();
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Converts an <see cref="ApplicationPage"/> type to an actual page derived from <see cref="Page"/>.
        /// </summary>
        /// <param name="pageType">The page type.</param>
        /// <param name="parameter">The optional parameter.</param>
        /// <returns></returns>
        private Page ConvertPage(ApplicationPage pageType, object parameter)
        {
            // Create the page based on the page type
            switch (pageType)
            {
                case ApplicationPage.Start:
                    {
                        return CreateStartPage();
                    }
                case ApplicationPage.Game:
                    {
                        // Parameter is expected to be the game settings
                        var gameSettings = (GameSettings)parameter;

                        return CreateGamePage(gameSettings);
                    }
                default:
                    {
                        // Should never get to here
                        return null!;
                    }
            }
        }

        /// <summary>
        /// Creates and returns a <see cref="StartPage"/>.
        /// </summary>
        /// <returns></returns>
        private StartPage CreateStartPage()
        {
            // Create the start page and set its data context to a new start page view model
            var startPage = new StartPage
            {
                DataContext = new StartPageViewModel()
            };

            return startPage;
        }

        /// <summary>
        /// Creates a <see cref="GamePage"/> with the specified game settings and returns it.
        /// </summary>
        /// <param name="gameSettings">The game settings.</param>
        /// <returns></returns>
        private GamePage CreateGamePage(GameSettings gameSettings)
        {
            // Create the game page and set its data context to a new game page view model with the
            // passed in game settings
            var gamePage = new GamePage
            {
                DataContext = new GamePageViewModel(gameSettings)
            };

            return gamePage;
        }

        #endregion
    }
}
