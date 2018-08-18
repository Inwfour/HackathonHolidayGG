using FluentAssertions;
using GG.Poll.BizDomains;
using GG.Poll.Models;
using GG.Poll.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace GG.Poll.Tests
{
    public class VoteTest
    {
        public Mock<IDataRepository<UniversalPoll>> pollDac { get; set; }
        public PollBizDomain pollBiz { get; set; }
        public IEnumerable<UniversalPoll> UniversalPolls { get; set; }

        public VoteTest()
        {
            var mock = new MockRepository(MockBehavior.Strict);
            pollDac = mock.Create<IDataRepository<UniversalPoll>>();

            pollBiz = new PollBizDomain(pollDac.Object);

            pollDac.Setup(dac => dac.Get(It.IsAny<Expression<Func<UniversalPoll, bool>>>()))
               .Returns<Expression<Func<UniversalPoll, bool>>>((expression) => UniversalPolls.FirstOrDefault(expression.Compile()));
            pollDac.Setup(dac => dac.List(It.IsAny<Expression<Func<UniversalPoll, bool>>>()))
               .Returns<Expression<Func<UniversalPoll, bool>>>((expression) => UniversalPolls.Where(expression.Compile()).ToList());
            pollDac.Setup(dac => dac.Create(It.IsAny<UniversalPoll>()));
            pollDac.Setup(dac => dac.UpdateOne(It.IsAny<Expression<Func<UniversalPoll, bool>>>(), It.IsAny<UniversalPoll>()));
            pollDac.Setup(dac => dac.DeleteOne(It.IsAny<Expression<Func<UniversalPoll, bool>>>()));
        }

        public static IEnumerable<object[]> CreateCompleteScenarios =>
        new List<object[]>
        {
            new object[] { new UniversalPoll { Id = "001", Title = "my vote", Choices = new List<Choice> { new Choice() } }, },
        };
        [Theory(DisplayName = "create complete")]
        [InlineData("user001", "poll001", "choice0014", 5)]
        public void CreateComplete(string username, string pollid, string choiceid, int rating)
        {
            pollBiz.Vote(username, pollid, choiceid, rating);
            pollDac.Verify(dac => dac.UpdateOne(It.IsAny<Expression<Func<UniversalPoll, bool>>>(), It.IsAny<UniversalPoll>()), Times.Once);
        }
    }
}
