using System;
using System.Collections.Generic;
using GRM.DataAccess.POCO;

namespace GRM.Services.Interfaces
{
    public interface IMusicContractService
    {
        IEnumerable<MusicContract> GetMusicContracts(string partner, DateTime effectiveDate);
    }
}