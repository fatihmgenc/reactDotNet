using DotNetAndReactSample.Models;
using DotNetAndReactSample.ViewModel;
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
        GoldListViewModel viewModel = GoldListViewModel.getViewModel();
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
        async public Task<ActionResult> GoldPriceList(int countPerPage, int page , int minPrice, int maxPrice)
        {
            viewModel.CountPerPage = countPerPage != 0 ? countPerPage : viewModel.CountPerPage;
            viewModel.MinPrice = minPrice != 0 ? minPrice : viewModel.MinPrice;
            viewModel.MaxPrice = maxPrice != 0 ? maxPrice : viewModel.MaxPrice;
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
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var streamReader = await response.Content.ReadAsStreamAsync();
                    result = (GoldToZlotyPriceRecordCollection)GoldPriceSerializer.Deserialize(streamReader);
                }
            }


            viewModel.Records = result.Records.Skip(viewModel.CountPerPage * (page - 1)).
                Take(viewModel.CountPerPage).
                Where(r=> double.Parse(r.Price)>=viewModel.MinPrice 
                && double.Parse(r.Price) < viewModel.MaxPrice).ToList();
            viewModel.CurrentPageNumber = page;
            viewModel.Count = result.Records.Count(r => double.Parse(r.Price) >= viewModel.MinPrice
                && double.Parse(r.Price) < viewModel.MaxPrice);
            return View(viewModel);

        }

        [HttpPost]
        public Task<ActionResult> GoldPriceList( )
        {
            var min = Request.Form["minPrice"];
            var max = Request.Form["maxPrice"];
            return GoldPriceList(viewModel.CountPerPage,viewModel.CurrentPageNumber, int.Parse(min), int.Parse(max));

        }
    }
}
