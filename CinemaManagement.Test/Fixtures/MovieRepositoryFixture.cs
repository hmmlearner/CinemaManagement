using CinemaManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Test.Fixtures
{
    public class MovieRepositoryFixture: IDisposable
    {
        public IMovieRepository Sut { get; private set; }

        public MovieRepositoryFixture()
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
    
    
}
