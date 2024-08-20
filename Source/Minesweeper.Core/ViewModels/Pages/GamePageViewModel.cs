using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Minesweeper.Core.Commands;
using Minesweeper.Core.Interfaces.Routing;
using Minesweeper.Core.Interfaces.Services;
using Minesweeper.Core.Routing;
using Minesweeper.Core.ValueObjects;
using Minesweeper.Core.ViewModels.Modals;

namespace Minesweeper.Core.ViewModels.Pages;

/// <summary>
/// The game page view model.
/// </summary>
[SuppressMessage(
    "Design",
    "CA1001",
    Justification = "This view model gets created by a value converter, through the framework, so it won't be disposed. TODO: find a solution.")]
public class GamePageViewModel : ViewModelBase
{
    #region Private members

    /// <summary>
    /// The timer that keeps track of time for this game.
    /// </summary>
    private readonly System.Timers.Timer _timer;

    /// <summary>
    /// The stopwatch that keeps track of time for this game.
    /// </summary>
    private readonly Stopwatch _stopWatch;

    #endregion

    #region Public properties

    /// <summary>
    /// The game field.
    /// </summary>
    public GameFieldViewModel GameField { get; }

    /// <summary>
    /// The time that has elapsed since the start of the game.
    /// </summary>
    public TimeSpan ElapsedTime { get; private set; } = TimeSpan.Zero;

    #region Commands

    /// <summary>
    /// The command that exits the game and navigates back to the start page.
    /// </summary>
    public IRelayCommand ExitCommand { get; }

    #endregion

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GamePageViewModel"/> class with
    /// the specified game settings.
    /// </summary>
    /// <param name="gameSettings">The game settings.</param>
    public GamePageViewModel(GameSettings gameSettings)
    {
        // Initialize the stopwatch and run the timer that will update the time every second
        _stopWatch = new Stopwatch();
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (o, e) => ElapsedTime = _stopWatch.Elapsed;

        // Create the game field and listen for when the game is started and is over
        GameField = new(gameSettings);

        GameField.GameStarted += (o, e) =>
        {
            // Start measuring the time when the game starts
            _stopWatch.Start();
            _timer.Start();
        };

        GameField.GameOver += async (o, e) =>
        {
            // Stop measuring the time when the game is over
            _stopWatch.Stop();
            _timer.Stop();

            var (modalTitle, modalMessage, modalConfirmText) = e switch
            {
                { PlayerWon: true, } => ("Congratulations", "You successfully finished the game.", "OK"),
                { PlayerWon: false, WasDirectBombClick: true, } => ("Game Over", "You clicked on a bomb.", "OK"),
                { PlayerWon: false, WasDirectBombClick: false } => ("Game Over", "You revealed a bomb.", "OK"),
                _ => throw new ArgumentOutOfRangeException(nameof(e), "Unexpected game over event arguments.")
            };

            // Show a modal with content based on the game result
            var modalViewModel = new ConfirmModalViewModel(modalTitle, modalMessage, modalConfirmText);

            // And wait until the modal is exited
            var modalService = IoC.Get<IModalService>();

            await modalService.ShowConfirmModalAsync(modalViewModel);

            // Handle game over
            OnGameOver();
        };

        ExitCommand = new RelayCommand(_ => OnGameOver());
    }

    #endregion

    #region Private helpers

    /// <summary>
    /// Handles game over and exiting the page.
    /// </summary>
    private void OnGameOver()
    {
        // Dispose of the timer if it exists (the game was started)
        _timer?.Dispose();

        // And navigate back to the start page
        var router = IoC.Get<IRouter>();
        router.NavigateTo(Routes.StartPageRoute);
    }

    #endregion
}
