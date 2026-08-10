using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Shared.EntitiesDto.Payam
{
    public class BeneficiaryDto
    {
        public string? Name { get; set; }
        public string? MainRelation { get; set; }
        public string? Relation { get; set; }
        public string? NationalCode { get; set; }
        public long Jari { get; set; }
        public long SarResid { get; set; }
        public long Moavagh { get; set; }
        public long Mashkook { get; set; }
        public long MandeTaahod { get; set; }
        public long TotalDebt { get; set; }

    }
}
