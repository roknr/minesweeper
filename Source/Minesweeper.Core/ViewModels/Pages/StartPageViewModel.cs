using Minesweeper.Core.Commands;
using Minesweeper.Core.Interfaces.Routing;
using Minesweeper.Core.Routing;
using Minesweeper.Core.ValueObjects;

namespace Minesweeper.Core.ViewModels.Pages;

/// <summary>
/// The start page view model.
/// </summary>
public class StartPageViewModel : ViewModelBase
{
    #region Public properties

    /// <summary>
    /// The game settings view model.
    /// </summary>
    public GameSettingsViewModel GameSettingsViewModel { get; private set; } = GameSettingsViewModel.IntermediateDifficulty;

    #region Commands

    /// <summary>
    /// The command that decreases the game field width.
    /// </summary>
    public IRelayCommand DecreaseWidthCommand { get; }

    /// <summary>
    /// The command that increases the game field width.
    /// </summary>
    public IRelayCommand IncreaseWidthCommand { get; }

    /// <summary>
    /// The command that decreases the game field height.
    /// </summary>
    public IRelayCommand DecreaseHeightCommand { get; }

    /// <summary>
    /// The command that decrease the game field height.
    /// </summary>
    public IRelayCommand IncreaseHeightCommand { get; }

    /// <summary>
    /// The command that changes the difficulty to beginner.
    /// </summary>
    public IRelayCommand BeginnerDifficultyCommand { get; }

    /// <summary>
    /// The command that changes the difficulty to intermediate.
    /// </summary>
    public IRelayCommand IntermediateDifficultyCommand { get; }

    /// <summary>
    /// The command that changes the difficulty to expert.
    /// </summary>
    public IRelayCommand ExpertDifficultyCommand { get; }

    /// <summary>
    /// The command that starts the game with the specified game settings.
    /// </summary>
    public IRelayCommand PlayCommand { get; }

    #endregion

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="StartPageViewModel"/> class.
    /// </summary>
    public StartPageViewModel()
    {
        // Set up the commands
        DecreaseWidthCommand = new RelayCommand(_ => GameSettingsViewModel.FieldWidth--);
        IncreaseWidthCommand = new RelayCommand(_ => GameSettingsViewModel.FieldWidth++);
        DecreaseHeightCommand = new RelayCommand(_ => GameSettingsViewModel.FieldHeight--);
        IncreaseHeightCommand = new RelayCommand(_ => GameSettingsViewModel.FieldHeight++);

        BeginnerDifficultyCommand = new RelayCommand(_ => GameSettingsViewModel = GameSettingsViewModel.BeginnerDifficulty);
        IntermediateDifficultyCommand = new RelayCommand(_ => GameSettingsViewModel = GameSettingsViewModel.IntermediateDifficulty);
        ExpertDifficultyCommand = new RelayCommand(_ => GameSettingsViewModel = GameSettingsViewModel.ExpertDifficulty);

        PlayCommand = new RelayCommand(_ =>
        {
            var router = IoC.Get<IRouter>();
            var argument = new GameSettings(GameSettingsViewModel.FieldWidth, GameSettingsViewModel.FieldHeight, GameSettingsViewModel.NumberOfBombs);

            router.NavigateTo(Routes.GamePageRoute, argument);
        });
    }

    #endregion
}
