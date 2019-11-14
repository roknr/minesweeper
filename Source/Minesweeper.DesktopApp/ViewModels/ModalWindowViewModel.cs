using Minesweeper.DesktopApp.Windows;

namespace Minesweeper.DesktopApp.ViewModels
{
    /// <summary>
    /// The view model for the modal window.
    /// </summary>
    public class ModalWindowViewModel : WindowViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModalWindowViewModel"/> class
        /// with the specified modal window.
        /// </summary>
        /// <param name="modalWindow">The modal window that this view model controls.</param>
        public ModalWindowViewModel(ModalWindow modalWindow) : base(modalWindow)
        {

        }
    }
}
