using System.Collections.Generic;

namespace GRM.Services
{
    public interface IPrinterService<T>
    {
        string Print(IEnumerable<T> results);
    }
}