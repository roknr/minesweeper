using System.Windows;
using Minesweeper.Core.Commands;
using Minesweeper.Core.ViewModels;

namespace Minesweeper.DesktopApp.ViewModels
{
    /// <summary>
    /// The base view model for all application window view models.
    /// </summary>
    public abstract class WindowViewModelBase : ViewModelBase
    {
        #region Protected members

        /// <summary>
        /// The window that this view model controls.
        /// </summary>
        protected readonly Window mWindow;

        #endregion

        #region Public properties

        /// <summary>
        /// The height of the window header.
        /// </summary>
        public int WindowHeaderHeight => 30;

        /// <summary>
        /// The window corner radius.
        /// </summary>
        public CornerRadius WindowCornerRadius => new CornerRadius(5);

        /// <summary>
        /// The thickness of the border where the window drop shadow is.
        /// </summary>
        public Thickness OuterMarginSizeThickness => new Thickness(10);

        #region Commands

        /// <summary>
        /// The command that minimizes the window.
        /// </summary>
        public IRelayCommand MinimizeCommand { get; }

        /// <summary>
        /// The command that closes the window.
        /// </summary>
        public IRelayCommand CloseCommand { get; }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowViewModelBase"/> class
        /// with the specified window.
        /// </summary>
        /// <param name="window">The window that this view model controls.</param>
        public WindowViewModelBase(Window window)
        {
            mWindow = window;

            MinimizeCommand = new RelayCommand(p => mWindow.WindowState = WindowState.Minimized);
            CloseCommand = new RelayCommand(p => mWindow.Close());
        }

        #endregion
    }
}
