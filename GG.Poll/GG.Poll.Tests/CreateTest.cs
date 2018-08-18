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
    public class CreateTest
    {
        public Mock<IDataRepository<UniversalPoll>> pollDac { get; set; }
        public PollBizDomain pollBiz { get; set; }
        public IEnumerable<UniversalPoll> UniversalPolls { get; set; }

        public CreateTest()
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
        [MemberData(nameof(CreateCompleteScenarios))]
        public void CreateComplete(UniversalPoll poll)
        {
            pollBiz.Create(poll);
            pollDac.Verify(dac => dac.Create(It.IsAny<UniversalPoll>()), Times.Once);
        }

        public static IEnumerable<object[]> CreateFailNoTitleScenarios =>
        new List<object[]>
        {
            new object[] { new UniversalPoll {Id = "001", Title = null }, "ไม่มีชื่อเรื่อง" },
            new object[] { new UniversalPoll {Id = "002", Title = "" }, "ไม่มีชื่อเรื่อง" },
            new object[] { new UniversalPoll {Id = "003", Title = " " }, "ไม่มีชื่อเรื่อง" },
            new object[] { new UniversalPoll { }, "ไม่มีชื่อเรื่อง" },
        };
        [Theory(DisplayName = "create fail: no title")]
        [MemberData(nameof(CreateFailNoTitleScenarios))]
        public void CreateFailNoTitle(UniversalPoll poll, string expectedMessage)
        {
            var message = string.Empty;
            try
            {
                pollBiz.Create(poll);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            message.Should().Be(expectedMessage);
            pollDac.Verify(dac => dac.Create(It.IsAny<UniversalPoll>()), Times.Never);
        }

        public static IEnumerable<object[]> CreateFailNoChoiceScenarios =>
        new List<object[]>
        {
            new object[] { new UniversalPoll {Id = "001", Title = "My poll", Choices = null }, "ไม่มีตัวเลือก" },
            new object[] { new UniversalPoll {Id = "002", Title = "My poll", Choices = Enumerable.Empty<Choice>() }, "ไม่มีตัวเลือก" },
        };
        [Theory(DisplayName = "create fail: no choice")]
        [MemberData(nameof(CreateFailNoChoiceScenarios))]
        public void CreateFailNoChoice(UniversalPoll poll, string expectedMessage)
        {
            var message = string.Empty;
            try
            {
                pollBiz.Create(poll);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            message.Should().Be(expectedMessage);
            pollDac.Verify(dac => dac.Create(It.IsAny<UniversalPoll>()), Times.Never);
        }
    }
}
