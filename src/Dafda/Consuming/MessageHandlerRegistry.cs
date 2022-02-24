using System;
using System.Collections.Generic;
using System.Linq;

namespace Dafda.Consuming
{
    internal sealed class MessageHandlerRegistry
    {
        private readonly List<MessageRegistration> _registrations = new List<MessageRegistration>();

        public void Register<TMessage, THandler>(string topic, string messageType, string version) 
            where THandler : IMessageHandler<TMessage> 
        {
            Register(
                handlerInstanceType: typeof(THandler),
                messageInstanceType: typeof(TMessage),
                topic: topic,
                messageType: messageType,
                version: version
            );
        }

        public MessageRegistration Register(Type handlerInstanceType, Type messageInstanceType, string topic, string messageType, string version)
        {
            var registration = new MessageRegistration(
                handlerInstanceType: handlerInstanceType,
                messageInstanceType: messageInstanceType,
                topic: topic,
                messageType: messageType,
                version: version
            );

            Register(registration);

            return registration;
        }

        public void Register(MessageRegistration registration)
        {
            _registrations.Add(registration);
        }

        public IEnumerable<string> GetAllSubscribedTopics() => _registrations.Select(x => x.Topic).Distinct();

        public IEnumerable<MessageRegistration> Registrations => _registrations;

        public MessageRegistration GetRegistrationFor(string messageType, string version = "1")
        {
            return _registrations.SingleOrDefault(x => x.MessageType == messageType && x.Version == version);
        }
    }
}
