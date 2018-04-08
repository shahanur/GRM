using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GRM.Interfaces;

namespace GRM.DataAccess.Repositories
{
    public class DistributionPartnerContractRepository<DistributionPartnerContract> : IRepository<DistributionPartnerContract>
    {
        private readonly IList<DistributionPartnerContract> _distributionPartnerContracts;
        public DistributionPartnerContractRepository(IDataContext<DistributionPartnerContract> dataContext)
        {
            _distributionPartnerContracts =
                dataContext.Read(Path.Combine(Environment.CurrentDirectory, "DistributionPartnerContract.txt"));
        }

        public IEnumerable<DistributionPartnerContract> Find(Func<DistributionPartnerContract, bool> predicate)
        {
            return _distributionPartnerContracts.Where(predicate);
        }
        
        IEnumerable<DistributionPartnerContract> IRepository<DistributionPartnerContract>.GetAll()
        {
            return _distributionPartnerContracts;
        }
    }
}