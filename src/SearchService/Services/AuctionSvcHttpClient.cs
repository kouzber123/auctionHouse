using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services
{
    public class AuctionSvcHttpClient
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _http;
        public AuctionSvcHttpClient(HttpClient http, IConfiguration config)
        {
            this._http = http;
            this._config = config;
        }
        public async Task<List<Item>> GetItemsForSearch()
        {

             var lastUpdated = await DB.Find<Item, string>()
                .Sort(x => x.Descending(u => u.UpdatedAt))
                .Project(x => x.UpdatedAt.ToString())
                .ExecuteFirstAsync();


            return await _http.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated);
        }
    }
}
