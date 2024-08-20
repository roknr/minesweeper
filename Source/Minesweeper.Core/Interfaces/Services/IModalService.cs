using Minesweeper.Core.ViewModels.Modals;

namespace Minesweeper.Core.Interfaces.Services;

/// <summary>
/// Defines modal service functionality.
/// </summary>
public interface IModalService
{
    /// <summary>
    /// Displays the confirm modal and returns a <see cref="Task"/> whose result represents
    /// the closing of the modal.
    /// </summary>
    /// <param name="viewModel">The confirm modal view model.</param>
    /// <returns></returns>
    Task ShowConfirmModalAsync(ConfirmModalViewModel viewModel);
}
