namespace Minesweeper.Core.Extensions;

/// <summary>
/// Provides array extension methods.
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// Executes the specified action for each adjacent element, relative to the specified
    /// row and column of the two dimensional array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The two dimensional array.</param>
    /// <param name="row">The row from where to check the adjacent elements.</param>
    /// <param name="column">The column from where to check the adjacent elements.</param>
    /// <param name="action">
    /// The action to execute.
    /// <para>First argument is the current adjacent element that is being processed.</para>
    /// <para>Second argument is the row of the current adjacent element that is being processed.</para>
    /// <para>Third argument is the column of the current adjacent element that is being processed.</para>
    /// </param>
    public static void ForEachAdjacent<T>(this T[,] array, int row, int column, Action<T, int, int> action)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentNullException.ThrowIfNull(action);

        // Get the number of rows and columns in the array
        var numberOfRows = array.GetLength(0);
        var numberOfColumns = array.GetLength(1);

        // Specified row and column must not be out of the array bounds
        if (row < 0 || row >= numberOfRows)
        {
            throw new ArgumentException("Specified row is out of array bounds.", nameof(row));
        }

        if (column < 0 || column >= numberOfColumns)
        {
            throw new ArgumentException("Specified column is out of array bounds.", nameof(column));
        }

        // To check adjacent elements, we have to start from 1 up and 1 left of specified row and column and
        // go 1 right and 1 down of specified row and column. However, we mustn't go out of array bounds if
        // specified row and/or column are on the edge of the array, so calculate the lower and upper index offset.

        //   0 1 2 3
        // 0 A _ _ _ --> start from (0,0) and go to (1,1)
        // 1 _ _ _ _
        // 2 _ _ B _ --> start from (1,1) and go to (3,3)
        // 3 _ _ _ _

        // If on the upper or lower edge of the array, don't go 1 up or 1 down
        var lowerRowOffset = (row == 0 ? 0 : -1);
        var upperRowOffset = (row == numberOfRows - 1 ? 0 : 1);

        for (var x = lowerRowOffset; x <= upperRowOffset; x++)
        {
            // If on the left most or right most edge of the array, don't go 1 left or 1 right
            var lowerColumnOffset = (column == 0 ? 0 : -1);
            var upperColumnOffset = (column == numberOfColumns - 1 ? 0 : 1);

            for (var y = lowerColumnOffset; y <= upperColumnOffset; y++)
            {
                // Skip the element of which the neighbors we are processing
                if (x == 0 && y == 0)
                {
                    continue;
                }

                // Invoke the specified action for each adjacent element with its row and column
                action(array[row + x, column + y], row + x, column + y);
            }
        }
    }
}
