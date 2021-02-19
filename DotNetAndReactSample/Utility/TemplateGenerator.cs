using DotNetAndReactSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAndReactSample.Utility
{
    public static class TemplateGenerator
    {
        public static string GetHTMLString(List<Stock> products)
        {
            var sb = new StringBuilder();
            sb.Append(
                @"
                 <html> 
                    <head></head>
                    <body>
                        <div class='header'><h1>This is generated PDF report !!!</h1></div>
                        <table align='center'>
                        <tr>
                            <th>StockCode</th>
                            <th>StockName</th>
                            <th>BrandName</th>
                            <th>Price</th>
                        </tr>");
                foreach (var item in products)
                {
                    sb.AppendFormat(@"<tr>
                            <th>{0}</th>
                            <th>{1}</th>
                            <th>{2}</th>
                            <th>{3}</th></tr>"
                            ,item.StockCode,item.StockName,item.BrandName,item.Price);
                }
            sb.Append(@"</body></html>");
            return sb.ToString();
        }
        
    }
}
