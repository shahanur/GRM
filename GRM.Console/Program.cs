using System;
using System.IO;
using GRM.DataAccess;
using GRM.DataAccess.POCO;
using GRM.DataAccess.Repositories;
using GRM.Interfaces;
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
                .BuildServiceProvider();

            //do the actual work 
            var musicContractRepository = serviceProvider.GetService<IRepository<MusicContract>>();
            var musicContracts = musicContractRepository.GetAll();

            var distributionPartnerRepository = serviceProvider.GetService<IRepository<DistributionPartnerContract>>();
            var partners = distributionPartnerRepository.GetAll();
            System.Console.ReadKey();
        }
    }
}
