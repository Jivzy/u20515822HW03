using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using u20515822_HW03.Models;

namespace u20515822_HW03.Controllers
{
    public class MaintainController : Controller
    {
        private LibraryEntities db = new LibraryEntities();
        // GET: Maintain
        public ActionResult Index(int? authorPage, int? typePage, int? borrowPage)
        {
            int pageSize = 10; // Number of students/books per page
            int authorPageNumber = authorPage ?? 1; // Current page for authors
            int typePageNumber = typePage ?? 1; // Current page for type
            int borrowPageNumber = borrowPage ?? 1; // Current page for borrows

            var authors = db.authors
                .OrderBy(a => a.authorId) //sorting logic here
                .Skip((authorPageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var types = db.types
                .OrderBy(t => t.typeId) // You can change the sorting logic here
                .Skip((typePageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var borrows = db.borrows
               .OrderBy(b => b.borrowId) // You can change the sorting logic here
               .Skip((borrowPageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToList();

            var model = new StudentBookViewModel
            {
                Authors = authors,
                Types = types,
                Borrows = borrows,
                AuthorsPageNumber = authorPageNumber,
                TypesPageNumber = typePageNumber,
                BorrowsPageNumber = borrowPageNumber,
                // Other properties
                TotalAuthorsPages = (int)Math.Ceiling(db.authors.Count() / (double)pageSize), // Calculate total pages for authors
                TotalTypesPages = (int)Math.Ceiling(db.types.Count() / (double)pageSize), // Calculate total pages for types
                TotalBorrowsPages = (int)Math.Ceiling(db.borrows.Count() / (double)pageSize) // Calculate total pages for borrows
            };

            return View(model);
        }

        public ActionResult CreateAuthor()
        {
            return View();
        }

        // POST: CreateAuthor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAuthor(authors author)
        {
            if (ModelState.IsValid)
            {
                db.authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index"); // Redirect to the list of authors
            }

            return View(author);
        }

        // GET: CreateType
        public ActionResult CreateType()
        {
            return View();
        }

        // POST: CreateType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateType(types types)
        {
            if (ModelState.IsValid)
            {
                db.types.Add(types);
                db.SaveChanges();
                return RedirectToAction("Index"); // Redirect to the list of books or any other appropriate page
            }

            return View(types);
        }

        // GET: CreateType
        public ActionResult CreateBorrow()
        {
            return View();
        }

        // POST: CreateType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBorrow(borrows borrows)
        {
            if (ModelState.IsValid)
            {
                db.borrows.Add(borrows);
                db.SaveChanges();
                return RedirectToAction("Index"); // Redirect to the list of books or any other appropriate page
            }

            return View(borrows);
        }

        public ActionResult DeleteType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            types type = db.types.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }

            return View(type); // Render the confirmation view
        }

        [HttpPost]
        public ActionResult DeleteTypeConfirmed(int id)
        {
            types type = db.types.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }

            // Update related books
            foreach (var book in type.books)
            {
                book.typeId = null; // or set it to another valid type
            }

            db.types.Remove(type);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult DeleteBorrow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            borrows borrow = db.borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }

            return View(borrow); // Render the confirmation view
        }
        [HttpPost]
        public ActionResult DeleteBorrowConfirmed(int id)
        {
            borrows borrow = db.borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }

            db.borrows.Remove(borrow);
            db.SaveChanges();

            return RedirectToAction("Index"); // Redirect to the borrow list after deletion
        }

        // GET: Author/Delete/5
        public ActionResult DeleteAuthor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            authors author = db.authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        // POST: Author/Delete
        [HttpPost]
        public ActionResult DeleteAuthorConfirmed(int id)
        {
            authors author = db.authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            db.authors.Remove(author);
            db.SaveChanges();

            return RedirectToAction("Index"); // Redirect to the author list after deletion
        }

        // POST : EditAuthor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuthor(StudentBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check for concurrency issues and update the borrow
                    db.Entry(model.Author).State = EntityState.Modified;
                    db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, "The record has been modified by another user. Please review and submit your changes again.");
                    return View(model);
                }
            }

            return View(model);
        }
        // GET : EditAuthor
        [HttpGet]
        public ActionResult EditAuthor(int id)
        {
            var author = db.authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }

            var model = new StudentBookViewModel
            {
                Author = author,
                OriginalAuthorName = author.name
            };

            return View(model);
        }

        // POST : EditType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditType(StudentBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check for concurrency issues and update the borrow
                    db.Entry(model.Type).State = EntityState.Modified;
                    db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, "The record has been modified by another user. Please review and submit your changes again.");
                    return View(model);
                }
            }

            return View(model);
        }

        // GET : EditTypes
        [HttpGet]
        public ActionResult EditType(int id)
        {
            var type = db.types.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }

            var model = new StudentBookViewModel
            {
                Type = type,
                OriginalTypeName = type.name
            };

            return View(model);
        }

        // GET : EditBorrow
        [HttpGet]
        public ActionResult EditBorrow(int id)
        {
            var borrow = db.borrows
                .Include(b => b.students)  // Load the students related entity
                .Include(b => b.books)     // Load the books related entity
                .FirstOrDefault(b => b.borrowId == id);

            if (borrow == null)
            {
                return HttpNotFound();
            }

            var model = new StudentBookViewModel
            {
                Borrow = borrow
            };

            return View(model);
        }

        // POST: EditBorrow
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBorrow(StudentBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check for concurrency issues and update the borrow
                    db.Entry(model.Borrow).State = EntityState.Modified;
                    db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, "The record has been modified by another user. Please review and submit your changes again.");
                    return View(model);
                }
            }

            return View(model);
        }

    }
}