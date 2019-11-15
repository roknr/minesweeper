using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Minesweeper.Core.Interfaces.Services;
using Minesweeper.Core.Models;

namespace Minesweeper.DesktopApp.Services
{
    /// <summary>
    /// A simple user settings service that uses JSON files and the user's local app
    /// data folder to store the settings.
    /// </summary>
    public class SimpleJsonUserSettingsService : IUserSettingsService
    {
        #region Private members

        /// <summary>
        /// The path to the file where the settings are stored.
        /// </summary>
        private readonly string mSettingsFilePath;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleJsonUserSettingsService"/> class.
        /// </summary>
        public SimpleJsonUserSettingsService()
        {
            // Create the user settings file path
            mSettingsFilePath = Path.Combine(Constants.LocalAppDataFolderPath, "userSettings.json");
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Reads the user settings form the local user's app data folder and returns them.
        /// If the settings don't exist, default settings are returned.
        /// </summary>
        /// <returns></returns>
        public UserSettingsModel Read()
        {
            // If the settings file does not exist, return default settings
            if (!File.Exists(mSettingsFilePath))
                return new UserSettingsModel();

            // Otherwise read the settings from the file, convert them to the settings model and return them
            var settingsJson = File.ReadAllText(mSettingsFilePath);
            var userSettings = JsonSerializer.Deserialize<UserSettingsModel>(settingsJson);

            return userSettings;
        }

        /// <summary>
        /// Asynchronously reads the user settings form the local user's app data folder and returns them.
        /// If the settings don't exist, default settings are returned.
        /// </summary>
        /// <returns></returns>
        public async Task<UserSettingsModel> ReadAsync()
        {
            // If the settings file does not exist, return default settings
            if (!File.Exists(mSettingsFilePath))
                return new UserSettingsModel();

            // Otherwise read the settings from the file, convert them to the settings model and return them
            var settingsJson = await File.ReadAllTextAsync(mSettingsFilePath);
            var userSettings = JsonSerializer.Deserialize<UserSettingsModel>(settingsJson);

            return userSettings;
        }

        /// <summary>
        /// Writes the specified user settings to a JSON file in the local user's app data folder.
        /// </summary>
        /// <param name="userSettings">The user settings to save.</param>
        public void Write(UserSettingsModel userSettings)
        {
            // Convert the settings into JSON
            var settingsJson = JsonSerializer.Serialize(userSettings, new JsonSerializerOptions { WriteIndented = true });

            // Create the application local app data directory if it does not yet exist
            Directory.CreateDirectory(Constants.LocalAppDataFolderPath);

            // And write the settings to the file (overwrite if file already exists or create if not)
            File.WriteAllText(mSettingsFilePath, settingsJson);
        }

        /// <summary>
        /// Asynchronously writes the specified user settings to a JSON file in the local user's app data folder.
        /// </summary>
        /// <param name="userSettings">The user settings to save.</param>
        public async Task WriteAsync(UserSettingsModel userSettings)
        {
            // Convert the settings into JSON
            var settingsJson = JsonSerializer.Serialize(userSettings, new JsonSerializerOptions { WriteIndented = true });

            // Create the application local app data directory if it does not yet exist
            Directory.CreateDirectory(Constants.LocalAppDataFolderPath);

            // And write the settings to the file (overwrite if file already exists or create if not)
            await File.WriteAllTextAsync(mSettingsFilePath, settingsJson);
        }

        #endregion
    }
}
