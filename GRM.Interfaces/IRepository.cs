using System;
using System.Collections.Generic;

namespace GRM.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> Find(Func<T,bool> predicate);
    }
}
