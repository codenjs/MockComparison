using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SystemUnderTest;

namespace MockComparison
{
    [TestClass]
    public class NSubstituteGeneralTests
    {
        [TestMethod]
        public void Mock_Behavior_Using_Multiple_Parameters()
        {
            var service = Substitute.For<IService>();

            service.GetData(Arg.Any<int>(), Arg.Any<int>())
                .Returns(callinfo => string.Format("{0}:{1}", callinfo.ArgAt<int>(0), callinfo.ArgAt<int>(1)));

            var result = service.GetData(1, 2);

            Assert.AreEqual("1:2", result);
        }

        [TestMethod]
        public void Mock_Stores_Single_Parameter()
        {
            var repository = Substitute.For<IRepository>();

            string dataSentToRepository = null;
            repository.Save(Arg.Do<string>(d => dataSentToRepository = d));

            repository.Save("1");

            Assert.AreEqual("1", dataSentToRepository);
        }
    }

    [TestClass]
    public class NSubstituteMessageRepositoryTests
    {
        private IMessageService _service = Substitute.For<IMessageService>();
        private IMessageRepository _repository = Substitute.For<IMessageRepository>();
        private List<string> _savedMessages = new List<string>();

        public NSubstituteMessageRepositoryTests()
        {
            _repository.Save(Arg.Do<string>(m => _savedMessages.Add(m)));
        }

        private void GivenServiceReturnsMessages(params string[] messages)
        {
            _service.GetMessages()
                .Returns(messages.ToList());
        }

        public void Process()
        {
            var processor = new MessageProcessor(_service, _repository);
            processor.Process();
        }

        [TestMethod]
        public void When_Service_Returns_One_Message_Then_One_String_Is_Saved()
        {
            GivenServiceReturnsMessages("message1");

            Process();

            Assert.AreEqual(1, _savedMessages.Count);
            Assert.AreEqual("message1", _savedMessages[0]);
        }

        [TestMethod]
        public void When_Service_Returns_Multiple_Messages_Then_Multiple_Strings_Are_Saved()
        {
            GivenServiceReturnsMessages("message1", "message2");

            Process();

            Assert.AreEqual(2, _savedMessages.Count);
            Assert.AreEqual("message1", _savedMessages[0]);
            Assert.AreEqual("message2", _savedMessages[1]);
        }
    }
}
