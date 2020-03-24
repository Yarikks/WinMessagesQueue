using System;
using System.Messaging;

namespace WinMessagesQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Write a name for queue: ");
            string queueName = Console.ReadLine();

            CreateNewQueue(queueName, "Info");

            Console.Write($"Write a message to {queueName}: ");
            string message = Console.ReadLine();

            SendMessageToQueue(queueName, message, "Simple message");

            ReceiveMessageFromQueue(queueName);


            Console.ReadLine();
        }

        static public void CreateNewQueue(string name, string label)
        {
            if (!MessageQueue.Exists($@".\private$\{name}"))
            {
                using (MessageQueue queue = MessageQueue.Create($@".\private$\{name}"))
                {
                    queue.Label = label;
                    Console.WriteLine($"Queue {name} was created.\nPath is: {queue.Path}");
                }
            }
            else
            {
                Console.WriteLine("This queue already exists.");
            }

        }

        static public void SendMessageToQueue(string QueueName, string message, string label)
        {
            if (MessageQueue.Exists($@".\private$\{QueueName}"))
            {
                MessageQueue queue = new MessageQueue($@".\private$\{QueueName}");
                queue.Send(message,label);
            }
            else
            {
                CreateNewQueue(QueueName, label);
            }
        }

        static public void ReceiveMessageFromQueue(string queueName)
        {
            MessageQueue queue = new MessageQueue($@".\private$\{queueName}");
            queue.Formatter = new XmlMessageFormatter(new string[] { "System.String" });

            int n = 0;
            foreach(Message mess in queue)
            {
                Console.WriteLine($"{n} message in queue: {mess.Body}");
                n++;
            }
        }
    }
}
