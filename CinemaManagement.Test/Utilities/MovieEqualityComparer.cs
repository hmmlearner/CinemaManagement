using CinemaManagement.Models;
using System.Diagnostics.CodeAnalysis;

namespace CinemaManagement.Test.Utilities
{
    internal class MovieEqualityComparer : IEqualityComparer<Movie>
    {
        public bool Equals(Movie x, Movie y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            // Compare individual properties for equality
            return x.Id == y.Id &&
                   x.Title == y.Title &&
                   x.Description == y.Description &&
                   x.Duration == y.Duration &&
                   x.Genre == y.Genre &&
                   x.ReleaseDate == y.ReleaseDate &&
                   Math.Abs(x.TicketPrice - y.TicketPrice) < 0.001; // Comparing doubles with a small tolerance

        }

        public int GetHashCode([DisallowNull] Movie obj)
        {
            return HashCode.Combine(obj.Id, obj.Title, obj.Description, obj.Duration, obj.Genre, obj.ReleaseDate, obj.TicketPrice);
        }
    }

}
