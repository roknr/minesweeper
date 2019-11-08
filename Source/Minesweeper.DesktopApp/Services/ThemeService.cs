using System;
using System.Windows;
using Minesweeper.Core.Enums;
using Minesweeper.Core.Interfaces.Services;

namespace Minesweeper.DesktopApp.Services
{
    /// <summary>
    /// The application theme service.
    /// </summary>
    public class ThemeService : IThemeService
    {
        #region Private constants

        /// <summary>
        /// The index at which the Colors{theme}.xaml are placed in the App.xaml file resource dictionary.
        /// </summary>
        private const int ColorsThemeIndex = 0;

        /// <summary>
        /// The index at which the Buttons{theme}.xaml are placed in the App.xam file resource dictionary.
        /// </summary>
        private const int ButtonsThemeIndex = 3;

        // The theme dictionary URIs
        private readonly Uri mColorsLightDictionaryUri = new Uri(@"Styles/ColorsLight.xaml", UriKind.Relative);
        private readonly Uri mButtonsLightDictionaryUri = new Uri(@"Styles/ButtonsLight.xaml", UriKind.Relative);
        private readonly Uri mColorsDarkDictionaryUri = new Uri(@"Styles/ColorsDark.xaml", UriKind.Relative);
        private readonly Uri mButtonsDarkDictionaryUri = new Uri(@"Styles/ButtonsDark.xaml", UriKind.Relative);

        #endregion

        #region Public methods

        /// <summary>
        /// Changes the application theme to the specified new theme.
        /// </summary>
        /// <param name="newTheme">The theme to change to.</param>
        public void ChangeTheme(ApplicationTheme newTheme)
        {
            // Create the new theme dictionaries
            var colorsThemeDictionary = new ResourceDictionary
            {
                Source = newTheme == ApplicationTheme.Light ? mColorsLightDictionaryUri : mColorsDarkDictionaryUri
            };
            var buttonsThemeDictionary = new ResourceDictionary
            {
                Source = newTheme == ApplicationTheme.Light ? mButtonsLightDictionaryUri : mButtonsDarkDictionaryUri
            };

            // And swap the old ones with the new ones
            SwapResourceDictionary(ColorsThemeIndex, colorsThemeDictionary);
            SwapResourceDictionary(ButtonsThemeIndex, buttonsThemeDictionary);
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Swaps the resource dictionary specified by the index argument with the specified 
        /// new resource dictionary.
        /// </summary>
        /// <param name="index">The index at which to swap.</param>
        /// <param name="newDictionary">The new resource dictionary.</param>
        private void SwapResourceDictionary(int index, ResourceDictionary newDictionary)
        {
            // Get the application dictionaries and swap the old one (at the index) with the new one
            var applicationDictionaries = Application.Current.Resources.MergedDictionaries;

            applicationDictionaries.RemoveAt(index);
            applicationDictionaries.Insert(index, newDictionary);
        }

        #endregion
    }
}
