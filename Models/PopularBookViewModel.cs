using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using u20515822_HW03.Controllers;

namespace u20515822_HW03.Models
{
    public class PopularBookViewModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int BorrowCount { get; set; }
    }
}