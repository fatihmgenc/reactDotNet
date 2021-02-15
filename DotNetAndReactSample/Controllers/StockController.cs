using DotNetAndReactSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DotNetAndReactSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : Controller
    {
        XmlSerializer serializer = new XmlSerializer(typeof(StockCollection));

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
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3000));
                FileStream fileStream = new FileStream("ExternalSource/menu.xml", FileMode.Open);
                result = (StockCollection)serializer.Deserialize(fileStream);
                result.StockCount = result.Stocks.Count;
                memoryCache.Set("result", result, cacheEntryOptions);
                fileStream.Close();
            }

            var tempResult = new StockCollection()
            {
                Stocks =  result.Stocks.Skip(countPerPage * (page - 1)).Take(countPerPage).ToList(),
                StockCount = result.StockCount
            };

            return tempResult;
                        

        }
    }
}
