using System;
using System.Collections.Generic;
using GRM.DataAccess.POCO;

namespace GRM.Services
{
    public interface IMusicContractService
    {
        IEnumerable<MusicContract> GetMusicContracts(string partner, DateTime effectiveDate);
    }
}