using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
    {
        private readonly IMapper _mapper;
        public AuctionDeletedConsumer(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("--> Consuming auction created " + context.Message.Id);
            Console.WriteLine("------------------------------------------------------------");

            var result = await DB.DeleteAsync<Item>(context.Message.Id);


            if (!result.IsAcknowledged)
                throw new MessageException(typeof(AuctionUpdated), "Problem deleting auction");

        }
    }
}
