using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IMapper _mapper;
        public AuctionCreatedConsumer(IMapper mapper)
        {
            this._mapper = mapper;
        }

        //class convention = include consumer
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("--> Consuming auction created " + context.Message.Id);
            Console.WriteLine("--> Consuming auction created " + context.Message.Model);
            Console.WriteLine("--> Consuming auction created " + context.Message.Color);
            Console.WriteLine("--> Consuming auction created " + context.Message.Mileage);
            Console.WriteLine("------------------------------------------------------------");
            //into - from
            var item = _mapper.Map<Item>(context.Message);

            if (item.Model == "Foo") throw new ArgumentException("Cannot sell cars with name foo");
            await item.SaveAsync();
        }
    }
}
