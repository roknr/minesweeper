using System.Diagnostics.CodeAnalysis;

namespace Minesweeper.Core.Routing;

/// <summary>
/// The <see cref="IEqualityComparer{T}"/> for <see cref="Route"/> objects.
/// </summary>
/// <remarks>
/// Reason for using an IEqualityComparer instead of implementing IEquatable on the
/// <see cref="Route"/> class is that the default comparison should be by reference,
/// but to uniquely identify (duplicate) routes, their properties should be compared
/// instead of the references.
/// </remarks>
public class RouteEqualityComparer : EqualityComparer<Route>
{
    /// <summary>
    /// Determines whether two objects of type <see cref="Route"/> are equal.
    /// </summary>
    /// <param name="x">The first route.</param>
    /// <param name="y">The second route.</param>
    /// <returns></returns>
    public override bool Equals(Route? x, Route? y)
    {
        if (x == null && y == null)
        {
            return true;
        }
        else if (x == null || y == null)
        {
            return false;
        }

        // Routes are equal (is a duplicate) if they have the same page type and depth
        return x.PageType == y.PageType && x.Depth == y.Depth;
    }

    /// <summary>
    /// Serves as a hash function for the specified object for hashing algorithms and
    /// data structures, such as a hash table.
    /// </summary>
    /// <param name="obj">The route for which to get a hash code.</param>
    public override int GetHashCode([DisallowNull] Route obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        // Generate the hash code from the properties that are used in comparison
        return HashCode.Combine(obj.PageType, obj.Depth);
    }
}
