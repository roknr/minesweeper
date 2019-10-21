using Minesweeper.Core.ValueObjects;

namespace Minesweeper.Core.ViewModels.Pages
{
    /// <summary>
    /// The game page view model.
    /// </summary>
    public class GamePageViewModel : ViewModelBase
    {
        /// <summary>
        /// The game settings.
        /// </summary>
        private readonly GameSettings mGameSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePageViewModel"/> class with
        /// the specified game settings.
        /// </summary>
        /// <param name="parameter">The game settings.</param>
        public GamePageViewModel(GameSettings gameSettings)
        {
            mGameSettings = gameSettings;
        }
    }
}
