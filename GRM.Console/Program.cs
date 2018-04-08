using System;
using System.IO;
using GRM.DataAccess;
using GRM.DataAccess.POCO;
using GRM.DataAccess.Repositories;
using GRM.Interfaces;
using GRM.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GRM.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello world!");

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDataContext<MusicContract>, DataContext<MusicContract>>()
                .AddTransient<IRepository<MusicContract>, MusicContractRepository<MusicContract>>()
                .AddTransient<IDataContext<DistributionPartnerContract>, DataContext<DistributionPartnerContract>>()
                .AddTransient<IRepository<DistributionPartnerContract>, DistributionPartnerContractRepository<DistributionPartnerContract>>()
                .AddTransient<IMusicContractService,MusicContractService>()
                .AddTransient<IPrinterService<MusicContract>,ConsolePrinterService<MusicContract>>()
                .BuildServiceProvider();

            //do the actual work 
            //var musicContractRepository = serviceProvider.GetService<IRepository<MusicContract>>();
            //var musicContracts = musicContractRepository.GetAll();

            //var distributionPartnerRepository = serviceProvider.GetService<IRepository<DistributionPartnerContract>>();
            //var partners = distributionPartnerRepository.GetAll();


            var musicContractService = serviceProvider.GetService<IMusicContractService>();
            var result = musicContractService.GetMusicContracts("YouTube", new DateTime(2012, 12, 25));
            var consolePrinterService = serviceProvider.GetService<IPrinterService<MusicContract>>();
            System.Console.WriteLine(consolePrinterService.Print(result));
            System.Console.ReadKey();
        }
    }
}
