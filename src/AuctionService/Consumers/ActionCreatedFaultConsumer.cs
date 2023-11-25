using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class ActionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
    {
        public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
        {
            Console.WriteLine("---> consuming faulty creation");

            var exception = context.Message.Exceptions.First();
            //replace the incorrect name for this
            if (exception.ExceptionType == "System.ArgumentException")
            {
                context.Message.Message.Model = "{{PLACEHOLDER NAME}}: PLEASE RENAME";
                await context.Publish(context.Message.Message);
            }
            else
            {
                Console.WriteLine("Not an argument exception - update error dashboard somewhere");
            }
        }
    }
}
