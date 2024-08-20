using Minesweeper.Core.Models;

namespace Minesweeper.Core.Interfaces.Services;

/// <summary>
/// Defines the user settings service functionality.
/// </summary>
public interface IUserSettingsService
{
    /// <summary>
    /// Writes the specified user settings to the application storage.
    /// </summary>
    /// <param name="userSettings">The user settings to save.</param>
    void Write(UserSettingsModel userSettings);

    /// <summary>
    /// Asynchronously writes the specified user settings to the application storage.
    /// </summary>
    /// <param name="userSettings">The user settings to save.</param>
    Task WriteAsync(UserSettingsModel userSettings);

    /// <summary>
    /// Reads the user settings form the application storage and returns them.
    /// </summary>
    /// <returns></returns>
    UserSettingsModel Read();

    /// <summary>
    /// Asynchronously reads the user settings from the application storage and returns them.
    /// </summary>
    /// <returns></returns>
    Task<UserSettingsModel> ReadAsync();
}
