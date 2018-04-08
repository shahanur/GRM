using System.Collections.Generic;

namespace GRM.Services.Interfaces
{
    public interface IPrinterService<T>
    {
        string Print(IEnumerable<T> results);
    }
}