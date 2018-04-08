using GRM.DataAccess;
using GRM.DataAccess.POCO;
using GRM.DataAccess.Repositories;
using GRM.Interfaces;
using GRM.Services;
using GRM.Services.Interfaces;
using GRM.Util;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GRM.Tests.Integration
{

    public class MusicContractServiceTests
    {
        private IMusicContractService _musicContractService;
        private IPrinterService<MusicContract> _consolePrinterService;

        public MusicContractServiceTests()
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDataContext<MusicContract>, DataContext<MusicContract>>()
                .AddTransient<IRepository<MusicContract>, MusicContractRepository<MusicContract>>()
                .AddTransient<IDataContext<DistributionPartnerContract>, DataContext<DistributionPartnerContract>>()
                .AddTransient<IRepository<DistributionPartnerContract>, DistributionPartnerContractRepository<DistributionPartnerContract>>()
                .AddTransient<IMusicContractService, MusicContractService>()
                .AddTransient<IPrinterService<MusicContract>, ConsolePrinterService<MusicContract>>()
                .BuildServiceProvider();

            _musicContractService = serviceProvider.GetService<IMusicContractService>();
            _consolePrinterService = serviceProvider.GetService<IPrinterService<MusicContract>>();
        }

        [Fact]
        public void When_User_Enters_ITunes_1st_March_2012()
        {
            var expected = @"Artist|Title|Usages|StartDate|EndDate
Monkey Claw|Black Mountain|digital download|1st Feb 2012|
Monkey Claw|Motor Mouth|digital download|1st Mar 2011|
Tinie Tempah|Frisky (Live from SoHo)|digital download|1st Feb 2012|
Tinie Tempah|Miami 2 Ibiza|digital download|1st Feb 2012|
";
            var contracts = _musicContractService.GetMusicContracts("iTunes", "1st March 2012".GetDateTimeFromOrdinal().Value);
            var output = _consolePrinterService.Print(contracts);

            Assert.Equal(expected, output);

        }

        [Fact]
        public void When_User_Enters_YouTube_1st_April_2012()
        {
            var expected = @"Artist|Title|Usages|StartDate|EndDate
Monkey Claw|Motor Mouth|streaming|1st Mar 2011|
Tinie Tempah|Frisky (Live from SoHo)|streaming|1st Feb 2012|
";

            var contracts = _musicContractService.GetMusicContracts("YouTube", "1st April 2012".GetDateTimeFromOrdinal().Value);
            var output = _consolePrinterService.Print(contracts);

            Assert.Equal(expected, output);

        }

        [Fact]
        public void When_User_Enters_YouTube_27th_Dec_2012()
        {
            var expected = @"Artist|Title|Usages|StartDate|EndDate
Monkey Claw|Iron Horse|streaming|1st June 2012|
Monkey Claw|Motor Mouth|streaming|1st Mar 2011|
Monkey Claw|Christmas Special|streaming|25th Dec 2012|31st Dec 2012
Tinie Tempah|Frisky (Live from SoHo)|streaming|1st Feb 2012|
";

            var contracts = _musicContractService.GetMusicContracts("YouTube", "27th Dec 2012".GetDateTimeFromOrdinal().Value);
            var output = _consolePrinterService.Print(contracts);

            Assert.Equal(expected, output);

        }


    }
}
