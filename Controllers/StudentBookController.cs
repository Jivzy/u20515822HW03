using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using u20515822_HW03.Models;

namespace u20515822_HW03.Controllers
{
    public class StudentBookController : Controller
    {
        private LibraryEntities db = new LibraryEntities();

        // GET: StudentBook
        public ActionResult Index(int? studentPage, int? bookPage)
        {
            int pageSize = 10; // Number of students/books per page
            int studentPageNumber = studentPage ?? 1; // Current page for students
            int bookPageNumber = bookPage ?? 1; // Current page for books

            var students = db.students
                .OrderBy(s => s.studentId) // You can change the sorting logic here
                .Skip((studentPageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var books = db.books
                .OrderBy(b => b.bookId) // You can change the sorting logic here
                .Skip((bookPageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new StudentBookViewModel
            {
                Students = students,
                Books = books,
                StudentPageNumber = studentPageNumber,
                BookPageNumber = bookPageNumber,
                // Other properties
                TotalStudentPages = (int)Math.Ceiling(db.students.Count() / (double)pageSize), // Calculate total pages for students
                TotalBookPages = (int)Math.Ceiling(db.books.Count() / (double)pageSize) // Calculate total pages for books
            };

            return View(model);
        }

        public ActionResult CreateStudent()
        {
            return View();
        }

        // POST: CreateStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudent(students student)
        {
            if (ModelState.IsValid)
            {
                db.students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index"); // Redirect to the list of students or any other appropriate page
            }

            return View(student);
        }

        // GET: CreateBook
        public ActionResult CreateBook()
        {
            return View();
        }

        // POST: CreateBook
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBook(books book)
        {
            if (ModelState.IsValid)
            {
                db.books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index"); // Redirect to the list of books or any other appropriate page
            }

            return View(book);
        }
    }
}