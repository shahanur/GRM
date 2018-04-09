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
    public class MusicContractRepositoryTests
    {
        private readonly Mock<IDataContext<MusicContract>> _mockDataContext;
        private readonly MusicContractRepository<MusicContract> _musicContractRepository;

        public MusicContractRepositoryTests()
        {
            _mockDataContext = new Mock<IDataContext<MusicContract>>();
            // set up mock version's method
            _mockDataContext.Setup(x => x.Read(It.IsAny<string>()))
                .Returns(new List<MusicContract> {new MusicContract() {Artist = "Test Artist"}});
            // create thing being tested with a mock dependency
            _musicContractRepository = new MusicContractRepository<MusicContract>(_mockDataContext.Object);

        }

        [Fact]
        public void When_Find_Is_Called_Verify_DataContext_Read_Method_Was_Called()
        {
            // act
            _musicContractRepository.Find(mc => mc.Artist.Contains("Test Artist"));

            //assert
            _mockDataContext.Verify(mdc => mdc.Read(It.IsAny<string>()));

        }

        [Fact]
        public void When_Find_Is_Called_It_Returns_TestArtist()
        {
            // act
            var result = _musicContractRepository.Find(mc => mc.Artist.Contains("Test Artist"));

            //assert
            Assert.Equal("Test Artist", result.First().Artist);
        }

        [Fact]
        public void When_Find_Is_Called_And_Null_Predicate_Is_Passed_Throws_Exception()
        {
            //assert
            Assert.Throws<ArgumentNullException>(() => _musicContractRepository.Find(null));
        }
    }
}
