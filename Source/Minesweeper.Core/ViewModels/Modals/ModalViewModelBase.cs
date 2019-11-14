namespace Minesweeper.Core.ViewModels.Modals
{
    /// <summary>
    /// The base view model for modals.
    /// </summary>
    public abstract class ModalViewModelBase : ViewModelBase
    {
        /// <summary>
        /// The modal title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModalViewModelBase"/> class
        /// with the specified title.
        /// </summary>
        /// <param name="title">The modal title.</param>
        public ModalViewModelBase(string title)
        {
            Title = title;
        }
    }
}
