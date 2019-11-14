using System.Windows;
using System.Windows.Input;
using Minesweeper.Core.Commands;
using Minesweeper.DesktopApp.Windows;

namespace Minesweeper.DesktopApp.ViewModels
{
    /// <summary>
    /// The view model for the main window.
    /// </summary>
    public class MainWindowViewModel : WindowViewModelBase
    {
        #region Public properties

        #region Commands

        /// <summary>
        /// The command that opens the icon context menu.
        /// </summary>
        public IRelayCommand OpenIconMenuCommand { get; }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class
        /// with the specified main window.
        /// </summary>
        /// <param name="mainWindow">The main window that this view model controls.</param>
        public MainWindowViewModel(MainWindow mainWindow) : base(mainWindow)
        {
            OpenIconMenuCommand = new RelayCommand(p => SystemCommands.ShowSystemMenu(mWindow, GetCurrentMousePosition()));
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Gets the current position of the mouse.
        /// </summary>
        /// <returns></returns>
        private Point GetCurrentMousePosition()
        {
            var position = Mouse.GetPosition(mWindow);

            return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);
        }

        #endregion
    }
}
