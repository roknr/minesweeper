using System.Windows;
using Minesweeper.DesktopApp.ViewModels;

namespace Minesweeper.DesktopApp.Windows;

/// <summary>
/// The generic modal window that can host different content (controls).
/// </summary>
public partial class ModalWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModalWindow"/> class.
    /// </summary>
    public ModalWindow()
    {
        InitializeComponent();

        DataContext = new ModalWindowViewModel(this);
    }
}
