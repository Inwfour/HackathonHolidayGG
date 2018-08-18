using GG.Poll.Models;
using GG.Poll.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GG.Poll.BizDomains
{
    public class PollBizDomain : IPollBizDomain
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

        public void Vote(string username, string pollId, string choiceId, int rating)
        {
            var poll = pollDac.Get(p => p.Id == pollId);
            var choice = poll.Choices.FirstOrDefault(c => c.Id == choiceId);
            var voter = choice.Voters.FirstOrDefault(v => v.Username == username);
            if (voter != null) voter.Rating = rating;
            else choice.Voters = choice.Voters.Concat(new List<Voter> { new Voter { Username = username, Rating = rating } });
            pollDac.UpdateOne(p => p.Id == pollId, poll);
        }

        public IEnumerable<UniversalPoll> ListAllPoll()
        {
            return pollDac.List(p => true);
        }

        public UniversalPoll GetPoll(string id)
        {
            return pollDac.Get(p => p.Id == id);
        }
    }
}
