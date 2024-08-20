using System.Windows;
using Minesweeper.Core.Interfaces.Services;
using Minesweeper.Core.ViewModels.Modals;
using Minesweeper.DesktopApp.Modals;

namespace Minesweeper.DesktopApp.Services;

/// <summary>
/// The application modal service.
/// </summary>
public class ModalService : IModalService
{
    /// <summary>
    /// Displays the confirm modal and returns a <see cref="Task"/> whose result represents
    /// the closing of the modal.
    /// </summary>
    /// <param name="viewModel">The confirm modal view model.</param>
    /// <returns></returns>
    public Task ShowConfirmModalAsync(ConfirmModalViewModel viewModel)
    {
        // The task that will be used to await the closing of the modal
        var modalClosedTaskCompletionSource = new TaskCompletionSource<bool>();

        Task.Run(() =>
        {
            // Run on the UI thread
            Application.Current.Dispatcher.Invoke(async () =>
            {
                try
                {
                    // Create the confirm modal
                    var confirmModal = new ConfirmModal();

                    // Show it and wait for it to be dismissed
                    await confirmModal.ShowAsync(viewModel);
                }
                finally
                {
                    // Let the caller know that the modal has been closed
                    modalClosedTaskCompletionSource.TrySetResult(true);
                }
            });
        });

        return modalClosedTaskCompletionSource.Task;
    }
}
