using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DotNetAndReactSample.Models
{
    [Serializable()]
    [XmlRoot("STOKLAR")]
    public class StockCollection
    {
        [XmlElement("STOK")]
        public List<Stock> Stocks { get; set; }
    }
}
