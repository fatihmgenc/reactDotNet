using DotNetAndReactSample.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAndReactSample.ViewModel
{
    public class GoldListViewModel
    {
        private  static GoldListViewModel goldListViewModel;
        private GoldListViewModel()
        {

        }

        public static GoldListViewModel getViewModel()
        {
            if (goldListViewModel == null)
            {
                goldListViewModel = new GoldListViewModel() { CountPerPage=10,CurrentPageNumber=1,MinPrice=0,MaxPrice=int.MaxValue};
                return goldListViewModel;
            }
            else
            {
                return goldListViewModel;
            }
        }

        public List<ZlotyPrice> Records { get; set; }

        public int Count { get; set; }

        public int CurrentPageNumber { get; set; }

        public int CountPerPage { get; set; }

        [BindProperty]
        public int MinPrice { get; set; }
        [BindProperty]
        public int MaxPrice { get; set; }
    }
}
