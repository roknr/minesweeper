namespace Minesweeper.Core.ViewModels.Modals;

/// <summary>
/// The confirm modal view model.
/// </summary>
public class ConfirmModalViewModel : ModalViewModelBase
{
    #region Public properties

    /// <summary>
    /// The modal message content.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// The modal's confirm button text.
    /// </summary>
    public string ConfirmText { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmModalViewModel"/> class
    /// with the specified title, message and confirm button text.
    /// </summary>
    /// <param name="title">The modal title.</param>
    /// <param name="message">The modal message content.</param>
    /// <param name="confirmText">The modal's confirm button text.</param>
    public ConfirmModalViewModel(string title, string message, string confirmText) : base(title)
    {
        Message = message;
        ConfirmText = confirmText;
    }

    #endregion
}
