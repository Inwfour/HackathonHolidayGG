using GG.Poll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GG.Poll.BizDomains
{
    public interface IPollBizDomain
    {
        void Create(UniversalPoll poll);
        IEnumerable<UniversalPoll> ListAllPoll();
        UniversalPoll GetPoll(string id);
        void Vote(string username, string pollId, string choiceId, int rating);
    }
}
