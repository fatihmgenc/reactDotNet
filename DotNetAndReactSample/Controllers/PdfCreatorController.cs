using DinkToPdf;
using DinkToPdf.Contracts;
using DotNetAndReactSample.Models;
using DotNetAndReactSample.Utility;
using DotNetAndReactSample.Utility.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetAndReactSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfCreatorController : Controller
    {
        private readonly IConverter _converter;
        private readonly IXmlToList<StockCollection> _xmlToCollection;
        private StockCollection _stockCollection;

        public PdfCreatorController(IConverter converter, IXmlToList<StockCollection> xmlToList)
        {
            _converter = converter;
            _xmlToCollection = xmlToList;
        }

        [HttpGet]
        public IActionResult CreatePDF(int countPerPage=10, int page=1)
        {
            _stockCollection = _xmlToCollection.Execute();

            List<Stock> list= _stockCollection.Stocks.Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Reports",
            };

            var objectSetting = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(list),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "style.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 12, Right = "Page [Page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 12, Center = "Report Footer", Line = true }
            };

            var pdf = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSetting }
            };

            var file = _converter.Convert(pdf);


            return File(file,"application/pdf");
        }
    }
}
