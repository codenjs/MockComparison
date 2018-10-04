using System.Collections.Generic;

namespace SystemUnderTest
{
    public interface IMessageService
    {
        List<string> GetMessages();
    }

    public interface IMessageRepository
    {
        void Save(string message);
    }

    public class MessageProcessor
    {
        private IMessageService _messageService;
        private IMessageRepository _messageRepository;

        public MessageProcessor(IMessageService messageService, IMessageRepository messageRepository)
        {
            _messageService = messageService;
            _messageRepository = messageRepository;
        }

        public void Process()
        {
            foreach (var message in _messageService.GetMessages())
                _messageRepository.Save(message);
        }
    }
}
