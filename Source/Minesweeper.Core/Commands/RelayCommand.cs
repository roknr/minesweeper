namespace Minesweeper.Core.Commands;

/// <summary>
/// A generic <see cref="IRelayCommand"/> implementation.
/// </summary>
public class RelayCommand : IRelayCommand
{
    #region Private members

    /// <summary>
    /// The action that the command performs on execution.
    /// </summary>
    private readonly Action<object?> _commandAction;

    /// <summary>
    /// The method whose result indicates whether it is possible to execute the command.
    /// </summary>
    private readonly Func<object?, bool>? _canExecute = null;

    #endregion

    #region Events

    /// <summary>
    /// The event that is fired when the command's <see cref="CanExecute(object)"/>
    /// method result changes.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="commandAction">The action that the command performs on execution.</param>
    /// <param name="canExecute">The method whose result indicates whether it is
    /// possible to execute the command.</param>
    public RelayCommand(Action<object?> commandAction, Func<object?, bool>? canExecute = null)
    {
        _commandAction = commandAction;
        _canExecute = canExecute;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Returns a value indicating whether the command can execute. If the command's <see cref="_canExecute"/>
    /// is not defined (wasn't provided in the constructor), the command can always execute.
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    public bool CanExecute(object? parameter)
    {
        // If the can execute method was not provided, the command can always execute
        if (_canExecute == null)
        {
            return true;
        }

        // Otherwise the result is dictated by the provided can execute method
        return _canExecute(parameter);
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    public void Execute(object? parameter)
    {
        // Execute the command by invoking the provided action method
        _commandAction(parameter);
    }

    /// <summary>
    /// Raises the command's <see cref="CanExecuteChanged"/> event, indicating
    /// that the <see cref="CanExecute(object)"/> method result might have changed.
    /// </summary>
    /// <remarks>
    /// TODO: Find a better way of raising the CanExecuteChanged event? Having a method to
    /// manually raise the internal event seems to defeat the purpose...
    /// </remarks>
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}
