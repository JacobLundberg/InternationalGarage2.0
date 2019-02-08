using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternationalGarage2_0.Models
{
    public class SearchMember
    {
        public string NameSearch { get; set; }
        
        public IEnumerable<Member> SearchResult { get; set; }
    }
}

