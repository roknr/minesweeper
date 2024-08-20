using System.Windows;
using System.Windows.Controls;
using Minesweeper.Core.Commands;
using Minesweeper.Core.ViewModels.Modals;
using Minesweeper.DesktopApp.Windows;

namespace Minesweeper.DesktopApp.Modals;

/// <summary>
/// The base modal control.
/// </summary>
public abstract class ModalBase : UserControl
{
    #region Protected members

    /// <summary>
    /// The dialog window in which this modal is displayed.
    /// </summary>
    protected ModalWindow DialogWindow { get; }

    #endregion

    #region Public properties

    #region Commands

    /// <summary>
    /// The command that closes the modal.
    /// </summary>
    public virtual IRelayCommand CloseCommand { get; }

    #endregion

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ModalBase"/> class.
    /// </summary>
    protected ModalBase()
    {
        DialogWindow = new ModalWindow();

        // Create the default close command by marking the modal as confirmed
        CloseCommand = new RelayCommand(_ => DialogWindow.DialogResult = true);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Shows this modal and returns a <see cref="Task"/> whose result indicates whether the
    /// modal was confirmed (true) or canceled (false).
    /// </summary>
    /// <param name="modalViewModel">The modal view model.</param>
    public Task<bool> ShowAsync(ModalViewModelBase modalViewModel)
    {
        var modalClosedTaskCompletionSource = new TaskCompletionSource<bool>();

        // Run on the UI thread
        Application.Current.Dispatcher.Invoke(() =>
        {
            var modalResult = false;

            try
            {
                // Set the modal window's content to this modal, its data context to the specified view model
                // and the modal title
                DialogWindow.Content = this;
                DataContext = modalViewModel;
                DialogWindow.Title = modalViewModel.Title;

                // Show the modal in the center of the main window
                DialogWindow.Owner = Application.Current.MainWindow;
                DialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                // If the modal was closed by confirming, the result is true, otherwise false
                if (DialogWindow.ShowDialog() == true)
                {
                    modalResult = true;
                }
            }
            finally
            {
                // Let the caller know that the modal has been closed, with the specified result
                modalClosedTaskCompletionSource.TrySetResult(modalResult);
            }
        });

        return modalClosedTaskCompletionSource.Task;
    }

    #endregion
}
