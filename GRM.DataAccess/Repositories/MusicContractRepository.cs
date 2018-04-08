using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GRM.Interfaces;

namespace GRM.DataAccess.Repositories
{
    public class MusicContractRepository<MusicContract>:IRepository<MusicContract>
    {
        private readonly IDataContext<MusicContract> _dataContext;
       
        public MusicContractRepository(IDataContext<MusicContract> dataContext)
        {
            _dataContext = dataContext;
        }

        private IEnumerable<MusicContract> GetAll()
        {
            return _dataContext.Read(Path.Combine(Environment.CurrentDirectory, "MusicContract.txt"));
        }

        public IEnumerable<MusicContract> Find(Func<MusicContract, bool> predicate)
        {
            return GetAll().Where(predicate);
        }
    }
}