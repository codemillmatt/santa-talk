using System;
using Xamarin.Forms;

namespace SantaTalk.Utils
{
    public class ChatMessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SentMessageTemplate { get; set; }
        public DataTemplate ReceivedMessageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((MessageModel)item).Type == MessageType.Received ? ReceivedMessageTemplate : SentMessageTemplate;
        }
    }
}
