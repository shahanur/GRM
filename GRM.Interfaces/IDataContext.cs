using System.Collections.Generic;

namespace GRM.Interfaces
{
    public interface IDataContext<T>
    {
        IList<T> Read(string filePath);
    }
}