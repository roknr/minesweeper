using System.Windows;
using Minesweeper.DesktopApp.IoC;
using Minesweeper.DesktopApp.Windows;

namespace Minesweeper.DesktopApp
{
    /// <summary>
    /// The Minesweeper desktop application.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The application starting point.
        /// </summary>
        /// <param name="e">Startup event arguments.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Do what base app needs to do
            base.OnStartup(e);

            // Set up the application
            SetupApplication();
        }

        #region Private helpers

        /// <summary>
        /// Sets up the application.
        /// </summary>
        private void SetupApplication()
        {
            // First set up dependency injection
            SetupIoC();

            // Lastly, create the main window and show it
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        /// <summary>
        /// Sets up the IoC container by passing it the <see cref="MinesweeperModule"/>
        /// service bindings.
        /// </summary>
        private void SetupIoC()
        {
            using var module = new MinesweeperModule();
            Core.IoC.Setup(module);
        }

        #endregion
    }
}
