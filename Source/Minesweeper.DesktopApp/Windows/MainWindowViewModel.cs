using System.Windows;
using System.Windows.Input;
using Minesweeper.Core.Commands;
using Minesweeper.Core.ViewModels;

namespace Minesweeper.DesktopApp.Windows
{
    /// <summary>
    /// The view model for the main window.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private members

        /// <summary>
        /// The window that this view model controls.
        /// </summary>
        private readonly MainWindow mWindow;

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
        /// The command that opens the icon context menu.
        /// </summary>
        public IRelayCommand OpenIconMenuCommand { get; }

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
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="mainWindow">The window that this view model controls.</param>
        public MainWindowViewModel(MainWindow mainWindow)
        {
            mWindow = mainWindow;

            OpenIconMenuCommand = new RelayCommand(p => SystemCommands.ShowSystemMenu(mWindow, GetCurrentMousePosition()));
            MinimizeCommand = new RelayCommand(p => mWindow.WindowState = WindowState.Minimized);
            CloseCommand = new RelayCommand(p => mWindow.Close());
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
