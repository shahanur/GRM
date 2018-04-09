using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GRM.Interfaces;

namespace GRM.DataAccess.Repositories
{
    public class DistributionPartnerContractRepository<DistributionPartnerContract> : IRepository<DistributionPartnerContract>
    {
        private const string DistributionpartnercontractTxt = "DistributionPartnerContract.txt";
        private readonly IDataContext<DistributionPartnerContract> _dataContext;
        
        public DistributionPartnerContractRepository(IDataContext<DistributionPartnerContract> dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<DistributionPartnerContract> Find(Func<DistributionPartnerContract, bool> predicate)
        {
            if(null == predicate)
                throw new ArgumentNullException();
            return GetAll().Where(predicate);
        }
        
        private IEnumerable<DistributionPartnerContract> GetAll()
        {
            return _dataContext.Read(Path.Combine(Environment.CurrentDirectory, DistributionpartnercontractTxt));
        }
    }
}