using System;
using System.IO;
using GRM.DataAccess;
using GRM.DataAccess.POCO;
using GRM.DataAccess.Repositories;
using GRM.Interfaces;
using GRM.Services;
using GRM.Util;
using Microsoft.Extensions.DependencyInjection;

namespace GRM.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                System.Console.WriteLine("You have not passed partner and effective date!");
                System.Console.ReadKey();
                return;
            }

            var partnerName = args[0];
            var effectiveDate = args[1];

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDataContext<MusicContract>, DataContext<MusicContract>>()
                .AddTransient<IRepository<MusicContract>, MusicContractRepository<MusicContract>>()
                .AddTransient<IDataContext<DistributionPartnerContract>, DataContext<DistributionPartnerContract>>()
                .AddTransient<IRepository<DistributionPartnerContract>,DistributionPartnerContractRepository<DistributionPartnerContract>>()
                .AddTransient<IMusicContractService, MusicContractService>()
                .AddTransient<IPrinterService<MusicContract>, ConsolePrinterService<MusicContract>>()
                .BuildServiceProvider();

            var musicContractService = serviceProvider.GetService<IMusicContractService>();

            var dateTime = effectiveDate.GetDateTimeFromOrdinal();
            if (dateTime != null)
            {
                var result = musicContractService.GetMusicContracts(partnerName, dateTime.Value);
                var consolePrinterService = serviceProvider.GetService<IPrinterService<MusicContract>>();
                System.Console.WriteLine(consolePrinterService.Print(result));
            }

            System.Console.ReadKey();
        }
    }
}
