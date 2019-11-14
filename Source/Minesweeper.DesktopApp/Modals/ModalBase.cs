using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Minesweeper.Core.Commands;
using Minesweeper.Core.ViewModels.Modals;
using Minesweeper.DesktopApp.Windows;

namespace Minesweeper.DesktopApp.Modals
{
    /// <summary>
    /// The base modal control.
    /// </summary>
    public abstract class ModalBase : UserControl
    {
        #region Protected members

        /// <summary>
        /// The dialog window in which this modal is displayed.
        /// </summary>
        protected readonly ModalWindow mDialogWindow;

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
        public ModalBase()
        {
            mDialogWindow = new ModalWindow();

            // Create the default close command by marking the modal as confirmed
            CloseCommand = new RelayCommand(p => mDialogWindow.DialogResult = true);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Shows this modal and returns a <see cref="Task"/> whose result indicates whether the
        /// modal was confirmed (true) or canceled (false).
        /// </summary>
        /// <param name="modalViewModel">The modal view model.</param>
        /// <returns></returns>
        public Task<bool> Show(ModalViewModelBase modalViewModel)
        {
            var modalClosedTaskCompletionSource = new TaskCompletionSource<bool>();

            // Run on the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                bool modalResult = false;

                try
                {
                    // Set the modal window's content to this modal, its data context to the specified view model
                    // and the modal title
                    mDialogWindow.Content = this;
                    DataContext = modalViewModel;
                    mDialogWindow.Title = modalViewModel.Title;

                    // Show the modal in the center of the main window
                    mDialogWindow.Owner = Application.Current.MainWindow;
                    mDialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                    // If the modal was closed by confirming, the result is true, otherwise false
                    if (mDialogWindow.ShowDialog() == true)
                        modalResult = true;
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
}
