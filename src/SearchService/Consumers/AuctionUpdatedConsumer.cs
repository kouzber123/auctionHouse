using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    /// <summary>
    ///   uses MONGO DB
    /// </summary>
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        private readonly IMapper _mapper;
        public AuctionUpdatedConsumer(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("--> Consuming auction updated " + context.Message.Id);
            Console.WriteLine("--> Consuming auction updated " + context.Message.Model);
            Console.WriteLine("--> Consuming auction updated " + context.Message.Color);
            Console.WriteLine("--> Consuming auction updated " + context.Message.Mileage);
            Console.WriteLine("---------------------------------------------------------");

            var item = _mapper.Map<Item>(context.Message);
            var result = await DB.Update<Item>()
                .Match(a => a.ID == context.Message.Id)
                .ModifyOnly(x => new
                {
                    x.Model,
                    x.Color,
                    x.Make,
                    x.Year,
                    x.Mileage
                }, item)
                .ExecuteAsync();

            if (!result.IsAcknowledged)
                throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb");
        }
    }
}
