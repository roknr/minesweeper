using System;
using System.Diagnostics;
using System.Timers;
using Minesweeper.Core.Commands;
using Minesweeper.Core.Interfaces.Routing;
using Minesweeper.Core.Interfaces.Services;
using Minesweeper.Core.Routing;
using Minesweeper.Core.ValueObjects;
using Minesweeper.Core.ViewModels.Modals;

namespace Minesweeper.Core.ViewModels.Pages
{
    /// <summary>
    /// The game page view model.
    /// </summary>
    public class GamePageViewModel : ViewModelBase
    {
        #region Private members

        /// <summary>
        /// The timer that keeps track of time for this game.
        /// </summary>
        private readonly Timer mTimer;

        /// <summary>
        /// The stopwatch that keeps track of time for this game.
        /// </summary>
        private readonly Stopwatch mStopWatch;

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
        /// <param name="parameter">The game settings.</param>
        public GamePageViewModel(GameSettings gameSettings)
        {
            // Initialize the stopwatch and run the timer that will update the time every second
            mStopWatch = new Stopwatch();
            mTimer = new Timer(1000);
            mTimer.Elapsed += (o, e) => ElapsedTime = mStopWatch.Elapsed;

            // Create the game field and listen for when the game is started and is over
            GameField = new GameFieldViewModel(gameSettings);
            GameField.GameStarted += (o, e) =>
            {
                // Start measuring the time when the game starts
                mStopWatch.Start();
                mTimer.Start();
            };
            GameField.GameOver += async (o, e) =>
            {
                // Stop measuring the time when the game is over
                mStopWatch.Stop();
                mTimer.Stop();

                // Show a modal with content based on the game result
                ConfirmModalViewModel modalViewModel;
                if (e.PlayerWon)
                    modalViewModel = new ConfirmModalViewModel("Congratulations", "You successfully finished the game", "OK");
                else
                    modalViewModel = new ConfirmModalViewModel("Game Over", "You clicked on a bomb", "OK");

                // And wait until the modal is exited
                var modalService = IoC.Get<IModalService>();
                await modalService.ShowConfirmModal(modalViewModel);

                // Handle game over
                OnGameOver();
            };

            ExitCommand = new RelayCommand(p => OnGameOver());
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Handles game over and exiting the page.
        /// </summary>
        private void OnGameOver()
        {
            // Dispose of the timer if it exists (the game was started)
            if (mTimer != null)
                mTimer.Dispose();

            // And navigate back to the start page
            var router = IoC.Get<IRouter>();
            router.NavigateTo(Routes.StartPageRoute);
        }

        #endregion
    }
}
