using Minesweeper.Core.Commands;
using Minesweeper.Core.Enums;
using Minesweeper.Core.Interfaces.Services;

namespace Minesweeper.Core.ViewModels
{
    /// <summary>
    /// The application view model.
    /// </summary>
    public class ApplicationViewModel : ViewModelBase
    {
        #region Public properties

        /// <summary>
        /// The application theme that is currently in use.
        /// </summary>
        public ApplicationTheme CurrentTheme { get; private set; } = ApplicationTheme.Light;

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
            ChangeThemeCommand = new RelayCommand(p =>
            {
                // Change to the non-current theme
                var newTheme = CurrentTheme == ApplicationTheme.Light ? ApplicationTheme.Dark : ApplicationTheme.Light;
                ChangeTheme(newTheme);
            });
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Changes the <see cref="CurrentTheme"/> to the specified new theme. Does nothing
        /// if the current application theme is already the newly specified one.
        /// </summary>
        /// <param name="newTheme">The theme to change to.</param>
        public void ChangeTheme(ApplicationTheme newTheme)
        {
            // Do nothing if trying to set the same theme
            if (newTheme == CurrentTheme)
                return;

            // Otherwise change the theme
            var themeService = IoC.Get<IThemeService>();
            themeService.ChangeTheme(newTheme);

            CurrentTheme = newTheme;
        }

        #endregion
    }
}
