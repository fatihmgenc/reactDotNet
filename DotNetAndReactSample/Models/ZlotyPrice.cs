using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DotNetAndReactSample.Models
{
    [Serializable()]
    public class ZlotyPrice
    {
        [System.Xml.Serialization.XmlElement("Data")]
        public DateTime Date { get; set; }

        [System.Xml.Serialization.XmlElement("Cena")]
        public string Price { get; set; }

    }
}
