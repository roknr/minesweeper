using Minesweeper.Core.Models;
using Minesweeper.Core.ViewModels;

namespace Minesweeper.Core.Extensions;

/// <summary>
/// Provides <see cref="UserSettingsViewModel"/> extension methods.
/// </summary>
public static class UserSettingsViewModelExtensions
{
    /// <summary>
    /// Converts the <see cref="UserSettingsViewModel"/> to a <see cref="UserSettingsModel"/>
    /// and returns it.
    /// </summary>
    /// <param name="viewModel">The user settings view model to convert.</param>
    /// <returns></returns>
    public static UserSettingsModel ToUserSettingsModel(this UserSettingsViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        return new UserSettingsModel
        {
            Theme = viewModel.Theme
        };
    }
}
