using GG.Poll.Models;
using GG.Poll.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GG.Poll.BizDomains
{
    public class PollBizDomain
    {
        public IDataRepository<UniversalPoll> pollDac { get; set; }

        public PollBizDomain(IDataRepository<UniversalPoll> pollDac)
        {
            this.pollDac = pollDac;
        }

        public void Create(UniversalPoll poll)
        {
            if (string.IsNullOrWhiteSpace(poll.Title))
            {
                throw new Exception("ไม่มีชื่อเรื่อง");
            }

            if (poll.Choices == null || !poll.Choices.Any())
            {
                throw new Exception("ไม่มีตัวเลือก");
            }

            pollDac.Create(poll);
        }
    }
}
