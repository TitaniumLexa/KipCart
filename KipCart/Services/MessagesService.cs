using System;
using System.Collections.Generic;
using System.Linq;

namespace KipCart.Services
{
    public interface IMessagesService
    {
        public void Register<T>(object context, object reciever, Action<T> action);
        public void Unregister(object reciever);
        public void SendMessage<T>(object context, T message);

    }

    public class MessagesService : IMessagesService
    {
        private readonly Dictionary<object, Dictionary<object, object>> subs = new Dictionary<object, Dictionary<object, object>>();
        public void Register<T>(object context, object reciever, Action<T> action)
        {
            if (subs.TryGetValue(context, out Dictionary<object, object> dictionary))
            {
                dictionary.TryAdd(reciever, action);
            }
            else
            {
                subs.Add(context, new Dictionary<object, object>() { { reciever, action } });
            }
        }

        public void Unregister(object reciever)
        {
            foreach (var key in subs.Keys)
            {
                subs[key].Remove(reciever);
            }
        }

        public void SendMessage<T>(object context, T message)
        {
            if (subs.TryGetValue(context, out Dictionary<object, object> dictionary))
            {
                foreach (var action in dictionary.Values.OfType<Action<T>>())
                {
                    action(message);
                }
            }
        }
    }
}
