namespace Minesweeper.Core.ViewModels;

/// <summary>
/// Class that serves mostly for locating view models (from IoC) through XAML.
/// </summary>
public class ViewModelLocator
{
    /// <summary>
    /// The singleton instance of the view model locator.
    /// </summary>
    public static ViewModelLocator Instance { get; } = new ViewModelLocator();

    /// <summary>
    /// The application view model.
    /// </summary>
    public ApplicationViewModel ApplicationViewModel { get; } = IoC.Get<ApplicationViewModel>();
}
