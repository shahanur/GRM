using System;
using System.Collections.Generic;

namespace GRM.DataAccess.POCO
{
    public class MusicContract
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public IList<DistributionType> Usages { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
