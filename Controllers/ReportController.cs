using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using u20515822_HW03.Models; 

namespace u20515822_HW03.Controllers
{
    public class ReportController : Controller
    {
        private LibraryEntities db = new LibraryEntities();

        // GET: ReportController
        public ActionResult PopularBooks(DateTime? takenDate, DateTime? broughtDate)
        {
            try
            {
                if (!takenDate.HasValue || !broughtDate.HasValue || takenDate == DateTime.MinValue || broughtDate == DateTime.MinValue)
                {
                    // Handle the case where one or both dates are invalid
                    return View("Error");
                }

                var PopularBooks = GetPopularBooks(takenDate.Value, broughtDate.Value);
                return View(PopularBooks);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return View("Error");
            }
        }

        private List<PopularBookViewModel> GetPopularBooks(DateTime startDate, DateTime endDate)
        {
            var popularBooks = db.borrows
                .Where(borrow => borrow.takenDate >= startDate && borrow.takenDate <= endDate)
                .GroupBy(borrow => borrow.bookId)
                .Select(group => new PopularBookViewModel
                {
                    BookId = group.Key ?? 0, 
                    BorrowCount = group.Count(),
                    BookName = GetBookName(group.Key ?? 0) // Get the book name
                })
                .OrderByDescending(book => book.BorrowCount)
                .ToList();

            return popularBooks;
        }

        // Helper method to get book names
        private string GetBookName(int bookId)
        {
            var book = db.books.FirstOrDefault(b => b.bookId == bookId);
            return book != null ? book.name : "Unknown";
        }

        public class PopularBookViewModel
        {
            public int BookId { get; set; }
            public string BookName { get; set; }
            public int BorrowCount { get; set; }
        }
    }
}

