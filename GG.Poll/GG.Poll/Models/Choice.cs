using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GG.Poll.Models
{
    public class Choice
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<Voter> Voters { get; set; }
    }
}
