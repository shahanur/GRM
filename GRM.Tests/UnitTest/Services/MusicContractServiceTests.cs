using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GRM.DataAccess;
using GRM.DataAccess.POCO;
using GRM.Interfaces;
using GRM.Services;
using Moq;
using Xunit;

namespace GRM.Tests.UnitTest.Services
{
    public class MusicContractServiceTests
    {
        private Mock<IRepository<MusicContract>> _musicContracRepository;
        private Mock<IRepository<DistributionPartnerContract>> _distributionPartnerContractRepository;
        private MusicContractService _musicContractService;

        public MusicContractServiceTests()
        {

            _distributionPartnerContractRepository = new Mock<IRepository<DistributionPartnerContract>>();
            _musicContracRepository = new Mock<IRepository<MusicContract>>();
            // set up mock version's method
            _distributionPartnerContractRepository.Setup(x => x.Find(It.IsAny<Func<DistributionPartnerContract, bool>>()))
                .Returns(new List<DistributionPartnerContract>
                {
                    new DistributionPartnerContract() { Partner = "YouTube", Usage = DistributionType.Streaming},
                    new DistributionPartnerContract() { Partner = "iTunes", Usage = DistributionType.Streaming}
                });

            _musicContracRepository.Setup(x => x.Find(It.IsAny<Func<MusicContract, bool>>()))
                .Returns(new List<MusicContract>
                {
                    new MusicContract() { Artist = "Tinie Tempah", Title = "Miami 2 Ibiza", Usages = new List<DistributionType>{DistributionType.DigitalDownload},StartDate = new DateTime(2012,02,01)},
                    new MusicContract() { Artist = "Monkey Claw", Title = "Christmas Special", Usages = new List<DistributionType>{DistributionType.Streaming},StartDate = new DateTime(2012,12,25), EndDate = new DateTime(2012,12,31)}
                });

            // create thing being tested with a mock dependency
            _musicContractService = new MusicContractService(_musicContracRepository.Object,_distributionPartnerContractRepository.Object);


        }

        [Fact]
        public void When_Partner_And_Effective_Date_Is_Passed_Should_Return_Valid_Contracts()
        {
            //act
            var result = _musicContractService.GetMusicContracts("YouTube", new DateTime(2012, 02, 01));

            //assert
            Assert.Equal("Monkey Claw", result.First().Artist);
        }

        [Fact]
        public void When_Null_Arguments_Are_Passed_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _musicContractService.GetMusicContracts(null, null));
            Assert.Throws<ArgumentNullException>(() => _musicContractService.GetMusicContracts("abc", null));
            Assert.Throws<ArgumentNullException>(() => _musicContractService.GetMusicContracts(null, new DateTime(2012,02,01)));
        }
    }
}
