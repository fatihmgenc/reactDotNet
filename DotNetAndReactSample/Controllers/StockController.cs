using DotNetAndReactSample.Models;
using DotNetAndReactSample.Utility.Services;
using DotNetAndReactSample.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DotNetAndReactSample.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StockController : Controller
    {

        private readonly IXmlToList<StockCollection> _xmlToCollection;
        private StockCollection _stockCollection;


        public StockController(IXmlToList<StockCollection> xmlToCollection)
        {
            _xmlToCollection = xmlToCollection;
        }

        [HttpGet]
        public StockCollection Get(int countPerPage, int page)
        {

            _stockCollection = _xmlToCollection.Execute();
            _stockCollection.StockCount = _stockCollection.Stocks.Count;

            var tempResult = new StockCollection()
            {
                Stocks = _stockCollection.Stocks.Skip(countPerPage * (page - 1)).Take(countPerPage).ToList(),
                StockCount = _stockCollection.StockCount
            };

            return tempResult;
        }

    }
}
