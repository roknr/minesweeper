using System.ComponentModel;

namespace Minesweeper.Core.ViewModels
{
    /// <summary>
    /// The base view model that implements the <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The event that is fired when a property's value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
