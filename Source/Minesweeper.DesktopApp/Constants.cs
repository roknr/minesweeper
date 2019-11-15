using System;

namespace Minesweeper.DesktopApp
{
    /// <summary>
    /// Provides the desktop application constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The path to the application's local app data folder.
        /// </summary>
        public static readonly string LocalAppDataFolderPath =
            @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\UpperBit\Minesweeper";
    }
}
