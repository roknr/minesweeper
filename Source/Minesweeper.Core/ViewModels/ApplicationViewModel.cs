using System;
using System.Threading.Tasks;
using Minesweeper.Core.Commands;
using Minesweeper.Core.Enums;
using Minesweeper.Core.Extensions;
using Minesweeper.Core.Interfaces.Services;
using Minesweeper.Core.Models;

namespace Minesweeper.Core.ViewModels
{
    /// <summary>
    /// The application view model.
    /// </summary>
    public class ApplicationViewModel : ViewModelBase
    {
        #region Public properties

        /// <summary>
        /// The user settings.
        /// </summary>
        public UserSettingsViewModel UserSettings { get; } = new UserSettingsViewModel();

        #region Commands

        /// <summary>
        /// The command that changes the current application theme.
        /// </summary>
        /// <remarks>
        /// TODO: maybe move this command somewhere else?
        /// </remarks>
        public IRelayCommand ChangeThemeCommand { get; }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationViewModel"/> class.
        /// </summary>
        public ApplicationViewModel()
        {
            // Listen for user settings changed
            UserSettings.PropertyChanged += (o, e) =>
            {
                // If the application theme was changed, make sure to update it across the application
                if (e.PropertyName.Equals(nameof(UserSettingsViewModel.Theme)))
                    OnThemeChanged(UserSettings.Theme);
            };

            ChangeThemeCommand = new RelayCommand(p =>
            {
                // Change to the non-current theme
                UserSettings.Theme = UserSettings.Theme == ApplicationTheme.Light ? ApplicationTheme.Dark : ApplicationTheme.Light;
            });
        }

        #endregion

        #region Public methods

        #region User settings

        /// <summary>
        /// Saves the current <see cref="UserSettings"/> to application settings storage.
        /// Ignores any possible errors.
        /// </summary>
        /// <returns></returns>
        public async Task SaveUserSettingsAsync()
        {
            try
            {
                var userSettingsService = IoC.Get<IUserSettingsService>();

                await userSettingsService.WriteAsync(UserSettings.ToUserSettingsModel());
            }
            catch { }
        }

        /// <summary>
        /// Saves the current <see cref="UserSettings"/> to application settings storage.
        /// Ignores any possible errors.
        /// </summary>
        /// <returns></returns>
        public void SaveUserSettings()
        {
            try
            {
                var userSettingsService = IoC.Get<IUserSettingsService>();

                userSettingsService.Write(UserSettings.ToUserSettingsModel());
            }
            catch { }
        }

        /// <summary>
        /// Reads the user settings from the application settings storage into the <see cref="UserSettings"/>.
        /// Uses the default user settings if not settings exist or if anything goes wrong.
        /// </summary>
        /// <returns></returns>
        public async Task ReadUserSettingsAsync()
        {
            // Use default settings if anything goes wrong
            UserSettingsModel userSettings = new UserSettingsModel();

            try
            {
                var userSettingsService = IoC.Get<IUserSettingsService>();

                userSettings = await userSettingsService.ReadAsync();
            }
            catch { }
            finally
            {
                UserSettings.SetFromUserSettingsModel(userSettings);
            }
        }

        /// <summary>
        /// Reads the user settings from the application settings storage into the <see cref="UserSettings"/>.
        /// Uses the default user settings if not settings exist or if anything goes wrong.
        /// </summary>
        /// <returns></returns>
        public void ReadUserSettings()
        {
            // Use default settings if anything goes wrong
            UserSettingsModel userSettings = new UserSettingsModel();

            try
            {
                var userSettingsService = IoC.Get<IUserSettingsService>();

                userSettings = userSettingsService.Read();
            }
            catch { }
            finally
            {
                UserSettings.SetFromUserSettingsModel(userSettings);
            }
        }

        #endregion

        #endregion

        #region Private helpers

        /// <summary>
        /// Handles the changing of the application theme by changing the current application
        /// theme to the specified new theme throughout the application.
        /// </summary>
        /// <param name="newTheme">The theme to change to.</param>
        private void OnThemeChanged(ApplicationTheme newTheme)
        {
            // Change the theme throughout the application
            var themeService = IoC.Get<IThemeService>();
            themeService.ChangeTheme(newTheme);
        }

        #endregion
    }
}
