using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using SantaTalk.Services;
using Xamarin.Forms;

namespace SantaTalk
{
    public class ChatPageViewModel : BaseViewModel
    {
        public ChatPageViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Messages.Add(_welcomeMessage);

            SendMessageCommand = new Command(async () =>
            {
                await SendMessage();
            });
        }


        public ICommand SendMessageCommand { get; }

        public ObservableCollection<MessageModel> Messages { get; set; }

        private ChatBotService _chatService = new ChatBotService();

        private string _message;
        public string CurrentMessage
        {
            get
            {
                return _message;
            }
            set
            {
                SetProperty(ref _message, value);
            }
        }

        private readonly MessageModel _welcomeMessage = new MessageModel
        {
            Message = "Welcome! \nThis is chat to share information about Christmas. My name is Charly. " +
            "I am automated elf bot and I do love Christmas and know ALL about it. Feel free to ask me",
            Type = MessageType.Received
        };

        private async Task SendMessage()
        {
            var currentMessage = new MessageModel();
            currentMessage.Message = CurrentMessage;
            currentMessage.Type = MessageType.Sent;
            Messages.Add(currentMessage);

            var response = await _chatService.SendMessage(CurrentMessage);
            CurrentMessage = string.Empty;

            var responseMessage = new MessageModel();
            responseMessage.Message = response;
            responseMessage.Type = MessageType.Received;
            Messages.Add(responseMessage);

        }
    }

    public class MessageModel
    {
        public string Message { get; set; }
        public MessageType Type { get; set; }

    }

    public enum MessageType
    {
        Sent = 0,
        Received =1,
    }
}
