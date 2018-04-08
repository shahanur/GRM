using System;
using System.Collections.Generic;
using System.Linq;
using GRM.DataAccess.POCO;
using GRM.Interfaces;

namespace GRM.Services
{
    public class MusicContractService : IMusicContractService
    {
        private readonly IRepository<MusicContract> _musicContractRepository;
        private readonly IRepository<DistributionPartnerContract> _distributionPartnerContractRepository;

        public MusicContractService(IRepository<MusicContract> musicContractRepository, IRepository<DistributionPartnerContract> distributionPartnerContractRepository)
        {
            _musicContractRepository = musicContractRepository;
            _distributionPartnerContractRepository = distributionPartnerContractRepository;
        }

        public IEnumerable<MusicContract> GetMusicContracts(string partner, DateTime effectiveDate)
        {
            var distributionPartnerContracts =
                _distributionPartnerContractRepository.Find(dpc => dpc.Partner.ToLower().Equals(partner.ToLower()));

            return distributionPartnerContracts.SelectMany(distributionPartnerContract =>
                _musicContractRepository.Find(mc =>
                    (mc.StartDate >= effectiveDate || (null != mc.EndDate && mc.EndDate <= effectiveDate)) &&
                    mc.Usages.Contains(distributionPartnerContract.Usage))).ToList();
        }
    }
}