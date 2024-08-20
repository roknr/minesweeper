using Minesweeper.Core.Enums;
using Minesweeper.Core.Models;

namespace Minesweeper.Core.ViewModels;

/// <summary>
/// The user setting view model.
/// </summary>
public class UserSettingsViewModel : ViewModelBase
{
    #region Private members

    /// <summary>
    /// The user settings default values.
    /// </summary>
    private readonly IReadOnlyDictionary<string, object> _defaults = new Dictionary<string, object>
    {
        { nameof(Theme), ApplicationTheme.Light }
    };

    #endregion

    #region Settings

    /// <summary>
    /// The application theme.
    /// </summary>
    public ApplicationTheme Theme { get; set; } = ApplicationTheme.Light;

    #endregion

    #region Public methods

    /// <summary>
    /// Sets the user settings by setting this view model's properties to the specified user settings model's
    /// corresponding properties. Also validates the properties for legal values and handles any non legal ones
    /// by setting them to their default values.
    /// </summary>
    /// <param name="userSettings">The user settings model to convert.</param>
    public void SetFromUserSettingsModel(UserSettingsModel userSettings)
    {
        ArgumentNullException.ThrowIfNull(userSettings);

        ValidateAndSetTheme(userSettings.Theme);
    }

    #endregion

    #region Private helpers

    /// <summary>
    /// Validates and sets the application theme.
    /// </summary>
    /// <param name="theme">The theme from the settings.</param>
    private void ValidateAndSetTheme(ApplicationTheme theme)
    {
        Theme = Enum.IsDefined(typeof(ApplicationTheme), theme)
            ? theme
            : GetDefaultValue<ApplicationTheme>(nameof(Theme));
    }

    /// <summary>
    /// Gets the default value for the property specified by the property name.
    /// </summary>
    /// <typeparam name="T">The type of the specified property.</typeparam>
    /// <param name="propertyName">The name of the property to get the default value for.</param>
    /// <returns></returns>
    private T GetDefaultValue<T>(string propertyName) => (T)_defaults[propertyName];

    #endregion
}
