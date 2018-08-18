using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GG.Poll.Models
{
    public class UniversalPoll
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string Owner { get; set; }
        public IEnumerable<Choice> Choices { get; set; }
    }
}
