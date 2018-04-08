using System;
using System.Collections.Generic;
using System.Linq;
using GRM.DataAccess;
using GRM.DataAccess.POCO;
using GRM.Interfaces;
using GRM.Services.Interfaces;

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

            var musicContracts = new List<MusicContract>();

            foreach (var distributionPartnerContract in distributionPartnerContracts)
            {
                var contracts = _musicContractRepository.Find(mc =>
                    EffectiveDateIsBiggerThanStartDate(effectiveDate, mc.StartDate) && EffectiveDateIsSmallerThanEndDate(effectiveDate, mc.EndDate) && mc.Usages.Contains(distributionPartnerContract.Usage));
                musicContracts.AddRange(contracts.Select(contract => new MusicContract
                {
                    Artist = contract.Artist,
                    Title = contract.Title,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    Usages = new List<DistributionType> {distributionPartnerContract.Usage}
                }));
            }

            return musicContracts.OrderBy(mc=>mc.Artist);
        }

        private bool EffectiveDateIsSmallerThanEndDate(DateTime effectiveDate, DateTime? endDate)
        {
            if (null == endDate)
                return true;
            return DateTime.Compare(effectiveDate, endDate.Value) <= 0;
        }

        private bool EffectiveDateIsBiggerThanStartDate(DateTime effectiveDate,DateTime? startDate)
        {
            if (null == startDate)
                return false;
            return DateTime.Compare(effectiveDate, startDate.Value) >= 0;
        }
    }
}