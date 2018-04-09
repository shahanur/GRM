using System;
using System.Collections.Generic;
using System.Linq;
using GRM.DataAccess.POCO;
using GRM.DataAccess.Repositories;
using GRM.Interfaces;
using Moq;
using Xunit;

namespace GRM.Tests.UnitTest.Repositories
{
    public class DistributionPartnerContractRepositoryTests
    {
        private readonly Mock<IDataContext<DistributionPartnerContract>> _mockDataContext;
        private readonly MusicContractRepository<DistributionPartnerContract> _distributionPartnerContractRepository;

        public DistributionPartnerContractRepositoryTests()
        {
            _mockDataContext = new Mock<IDataContext<DistributionPartnerContract>>();
            // set up mock version's method
            _mockDataContext.Setup(x => x.Read(It.IsAny<string>()))
                .Returns(new List<DistributionPartnerContract> {new DistributionPartnerContract() {Partner = "YouTube"}});
            // create thing being tested with a mock dependency
            _distributionPartnerContractRepository = new MusicContractRepository<DistributionPartnerContract>(_mockDataContext.Object);

        }

        [Fact]
        public void When_Find_Is_Called_Verify_DataContext_Read_Method_Was_Called()
        {
            // act
            _distributionPartnerContractRepository.Find(mc => mc.Partner.Contains(It.IsAny<string>()));

            //assert
            _mockDataContext.Verify(mdc => mdc.Read(It.IsAny<string>()));

        }

        [Fact]
        public void When_Find_Is_Called_It_Returns_YouTube()
        {
            // act
            var result = _distributionPartnerContractRepository.Find(mc => mc.Partner.Contains("YouTube"));

            //assert
            Assert.Equal("YouTube", result.First().Partner);
        }

        [Fact]
        public void When_Find_Is_Called_And_Null_Predicate_Is_Passed_Throws_Exception()
        {
            //assert
            Assert.Throws<ArgumentNullException>(() => _distributionPartnerContractRepository.Find(null));
        }
    }
}
