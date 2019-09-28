using System.Windows.Input;

namespace Minesweeper.Core.Commands
{
    /// <summary>
    /// Defines a relay command.
    /// </summary>
    public interface IRelayCommand : ICommand
    {
        /// <summary>
        /// Raises the command's <see cref="ICommand.CanExecuteChanged"/> event, indicating
        /// that the <see cref="ICommand.CanExecute(object)"/> method result might have changed.
        /// </summary>
        /// <remarks>
        /// TODO: Find a better way of raising the CanExecuteChanged event? Having a method to
        /// manually raise the internal event seems to defeat the purpose...
        /// </remarks>
        void RaiseCanExecuteChanged();
    }
}
