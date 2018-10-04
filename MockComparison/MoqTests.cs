using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SystemUnderTest;

namespace MockComparison
{
    [TestClass]
    public class MoqGeneralTests
    {
        [TestMethod]
        public void Mock_Behavior_Using_Multiple_Parameters()
        {
            var service = new Mock<IService>();

            service.Setup(s => s.GetData(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((userId, qty) => string.Format("{0}:{1}", userId, qty));

            var result = service.Object.GetData(1, 2);

            Assert.AreEqual("1:2", result);
        }

        [TestMethod]
        public void Mock_Stores_Single_Parameter()
        {
            var repository = new Mock<IRepository>();

            string dataSentToRepository = null;
            repository.Setup(r => r.Save(It.IsAny<string>()))
                .Callback<string>(d => dataSentToRepository = d);

            repository.Object.Save("1");

            Assert.AreEqual("1", dataSentToRepository);
        }
    }

    [TestClass]
    public class MoqMessageRepositoryTests
    {
        private Mock<IMessageService> _service = new Mock<IMessageService>();
        private Mock<IMessageRepository> _repository = new Mock<IMessageRepository>();
        private List<string> _savedMessages = new List<string>();

        public MoqMessageRepositoryTests()
        {
            _repository.Setup(r => r.Save(It.IsAny<string>()))
                .Callback<string>(m => _savedMessages.Add(m));
        }

        private void GivenServiceReturnsMessages(params string[] messages)
        {
            _service.Setup(s => s.GetMessages())
                .Returns(messages.ToList());
        }

        public void Process()
        {
            var processor = new MessageProcessor(_service.Object, _repository.Object);
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
