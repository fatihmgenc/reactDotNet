using DotNetAndReactSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DotNetAndReactSample.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StockController : Controller
    {

        private readonly IMemoryCache memoryCache;

        public StockController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public StockCollection Get(int countPerPage, int page)
        {

            StockCollection result;
            bool isExist = memoryCache.TryGetValue("result", out result);

            if (!isExist)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(StockCollection));

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3000));
                FileStream fileStream = new FileStream("ExternalSource/menu.xml", FileMode.Open);
                result = (StockCollection)serializer.Deserialize(fileStream);
                result.StockCount = result.Stocks.Count;
                memoryCache.Set("result", result, cacheEntryOptions);
                fileStream.Close();
            }

            var tempResult = new StockCollection()
            {
                Stocks = result.Stocks.Skip(countPerPage * (page - 1)).Take(countPerPage).ToList(),
                StockCount = result.StockCount
            };

            return tempResult;
        }
        [HttpGet]
        async public Task<ActionResult> GoldPriceList(int countPerPage=10, int page=1)
        {
        
            GoldToZlotyPriceRecordCollection result;
            bool isExist = memoryCache.TryGetValue("result", out result);

            if (!isExist)
            {
                XmlSerializer GoldPriceSerializer = new XmlSerializer(typeof(GoldToZlotyPriceRecordCollection));
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("http://api.nbp.pl/api/cenyzlota/2020-01-01/2021-01-01?format=xml")
                };
                using (var response =  await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var streamReader = await response.Content.ReadAsStreamAsync();
                    result = (GoldToZlotyPriceRecordCollection)GoldPriceSerializer.Deserialize(streamReader);
                }
            }

            var model = new GoldToZlotyPriceRecordCollection()
            {
                Records = result.Records.Skip(countPerPage * (page - 1)).Take(countPerPage).ToList(),
                PageCount = result.Records.Count / countPerPage,
                CurrentPageNumber = page,
            };

            return View(model);

        }
    }
}
