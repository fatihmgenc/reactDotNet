using DotNetAndReactSample.Models;
using Microsoft.AspNetCore.Mvc;
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
        
        [HttpGet]
        public IEnumerable<Stock> Get(int countPerPage, int page)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(StockCollection));

            using(FileStream fileStream = new FileStream("ExternalSource/menu.xml", FileMode.Open))
            {
                StockCollection result = (StockCollection) serializer.Deserialize(fileStream);
                return result.Stocks.Skip(countPerPage*(page-1)).Take(countPerPage);
            }

        }
    }
}
