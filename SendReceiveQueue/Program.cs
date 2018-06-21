using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendReceiveQueue
{
    class Program
    {
        static string connectionString = "[REPLACE-WITH-CONNECTION-STRING]";
        static string queueName = "SBLab2Queue";
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        static async Task MainAsync()
        {
            var factory = MessagingFactory.CreateFromConnectionString(connectionString);
            //create a receiver on the queue
            var receiver = await factory.CreateMessageReceiverAsync(queueName);
            //create a sender on the queue
            var sender = await factory.CreateMessageSenderAsync(queueName);

            //start message pump to listen for message on the queue
            receiver.OnMessageAsync(async receivedMessage =>
            {
                //trace out the MessageId property of received message
                Console.WriteLine("Receiving message - {0}", receivedMessage.MessageId);
                await receivedMessage.CompleteAsync();
            }, new OnMessageOptions() { AutoComplete = false });

            //send 3 message to the queue
            Console.WriteLine("Sending Message 1");
            await sender.SendAsync(new BrokeredMessage("Hello World!") { MessageId = "deadbeef-dead-beef-dead-beef00000075" });
            Console.WriteLine("Sending Message 2");
            await sender.SendAsync(new BrokeredMessage("Hello World!") { MessageId = "deadbeef-dead-beef-dead-beef00000075" });
            Console.WriteLine("Sending Message 3");
            await sender.SendAsync(new BrokeredMessage("Hello World!") { MessageId = "deadbeef-dead-beef-dead-beef00000075" });

            await Task.WhenAny(
                Task.Run(() => Console.ReadKey()),
                Task.Delay(TimeSpan.FromSeconds(30))
            );
        }
    }
}
