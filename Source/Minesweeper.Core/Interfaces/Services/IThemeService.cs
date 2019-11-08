using Minesweeper.Core.Enums;

namespace Minesweeper.Core.Interfaces.Services
{
    /// <summary>
    /// Defines the application theme service functionality.
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        /// Changes the application theme to the specified new theme.
        /// </summary>
        /// <param name="newTheme">The theme to change to.</param>
        void ChangeTheme(ApplicationTheme newTheme);
    }
}
