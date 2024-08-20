using System.ComponentModel;

namespace Minesweeper.Core.ViewModels;

/// <summary>
/// The base view model that implements the <see cref="INotifyPropertyChanged"/>.
/// </summary>
public abstract class ViewModelBase : INotifyPropertyChanged
{
    /// <summary>
    /// The event that is fired when a property's value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event for the property specified by the property name.
    /// </summary>
    /// <param name="propertyName">The name of the property for which to raise the event.</param>
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
