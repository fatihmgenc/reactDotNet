using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAndReactSample.Models
{
    [Serializable()]
    public class Stock
    {
        [System.Xml.Serialization.XmlElement("STOK_KOD")]
        public string StockCode { get; set; }

        [System.Xml.Serialization.XmlElement("STOK_AD")]
        public string StockName { get; set; }

        [System.Xml.Serialization.XmlElement("MARKA_AD")]
        public string BrandName { get; set; }

        [System.Xml.Serialization.XmlElement("BAYI_OZEL_FIYAT")]
        public string Price { get; set; }
    }
}
