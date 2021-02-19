using DotNetAndReactSample.Models;
using DotNetAndReactSample.Utility.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Xml.Serialization;

namespace DotNetAndReactSample.Utility
{
    public class XmlToCollectionReader : IXmlToList<StockCollection>
    {
        private readonly IMemoryCache _memoryCache;

        public XmlToCollectionReader(IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
        }

        public StockCollection Execute()
        {
            StockCollection result;
            
            bool isExist = _memoryCache.TryGetValue("result", out result);
            if (!isExist)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3000));
                XmlSerializer serializer = new XmlSerializer(typeof(StockCollection));
                FileStream fileStream = new FileStream("ExternalSource/menu.xml", FileMode.Open);
                result = (StockCollection) serializer.Deserialize(fileStream);
                fileStream.Close();
                _memoryCache.Set("result", result, cacheEntryOptions);
            }
            result.StockCount = result.Stocks.Count;
            return result;
        }
    }
}
