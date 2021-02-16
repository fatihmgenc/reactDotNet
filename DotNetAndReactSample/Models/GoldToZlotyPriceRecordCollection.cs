using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DotNetAndReactSample.Models
{
    [Serializable()]
    [XmlRoot("ArrayOfCenaZlota")]
    public class GoldToZlotyPriceRecordCollection
    {
        [System.Xml.Serialization.XmlElement("CenaZlota")]
        public List<ZlotyPrice>  Records { get; set; }

    }
}
