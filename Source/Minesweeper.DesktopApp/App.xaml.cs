using System.Windows;
using Minesweeper.Core.ViewModels;
using Minesweeper.DesktopApp.IoC;
using Minesweeper.DesktopApp.Windows;

namespace Minesweeper.DesktopApp;

/// <summary>
/// The Minesweeper desktop application.
/// </summary>
public partial class App : Application
{
    #region Application startup and shutdown

    /// <summary>
    /// The application starting point.
    /// </summary>
    /// <param name="e">Startup event arguments.</param>
    protected override async void OnStartup(StartupEventArgs e)
    {
        // Do what base app needs to do
        base.OnStartup(e);

        // Set up the application
        await SetupApplicationAsync();
    }

    /// <summary>
    /// The application shutdown point.
    /// </summary>
    /// <remarks>
    /// This method must not be asynchronous. Since the method is void and the
    /// application is shutting down, it will not await the method.
    /// </remarks>
    /// <param name="e">The application exit event arguments.</param>
    protected override void OnExit(ExitEventArgs e)
    {
        // Do what base app needs to do
        base.OnExit(e);

        // Save the user settings on exit
        var applicationViewModel = Core.IoC.Get<ApplicationViewModel>();
        applicationViewModel.SaveUserSettings();
    }

    #endregion

    #region Private helpers

    /// <summary>
    /// Sets up the application.
    /// </summary>
    private static async Task SetupApplicationAsync()
    {
        // First set up dependency injection
        SetupIoC();

        // Set up the initial user settings
        var applicationViewModel = Core.IoC.Get<ApplicationViewModel>();
        await applicationViewModel.ReadUserSettingsAsync();

        // Lastly, create the main window and show it
        Current.MainWindow = new MainWindow();
        Current.MainWindow.Show();
    }

    /// <summary>
    /// Sets up the IoC container by passing it the <see cref="MinesweeperModule"/>
    /// service bindings.
    /// </summary>
    private static void SetupIoC()
    {
        using var module = new MinesweeperModule();
        Core.IoC.Setup(module);
    }

    #endregion
}
