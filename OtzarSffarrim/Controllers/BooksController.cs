using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using OtzarSffarrim.Data;
using OtzarSffarrim.Models;

namespace OtzarSffarrim.Controllers
{
    public class BooksController : Controller
    {
        private readonly OtzarSffarrimContext _context;

        public BooksController(OtzarSffarrimContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string message="fghjhklk;'")
        {
            ViewBag.Message = message;
            var otzarSffarrimContext = _context.Book.Include(b => b.Shelf);
            return View(await otzarSffarrimContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["ShelfId"] = new SelectList(_context.Shelf, "ShelfId", "ShelfId");



            ViewData["Categories"] = new SelectList(_context.Library
                                        .Select(library => library.LibraryCategory)
                                        .Distinct()
                                        .ToList());


            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookName,BookCategory,Width,Height,ShelfId")] Book book)
        {
            var shelfId = book.ShelfId;
            var releventShelves = _context.Shelf
                        .Join(
                            _context.Library,
                            shelf => shelf.LibraryId,
                            library => library.LibraryId,
                            (shelf, library) => new { shelf, library })
                        .Where(sl => sl.library.LibraryCategory == book.BookCategory)
                        .Select(sl => sl.shelf.ShelfId)
                        .ToList();
            var freeWidth = _context.Shelf.Find(book.ShelfId).FreeWidth;
            var freeHeight = _context.Shelf.Find(book.ShelfId).Height;
            List<string> category = new List<string>();
            category.Add(book.BookCategory);

            bool exists = releventShelves.Contains(shelfId);

            if ( exists == false || book.Width > freeWidth || book.Height > freeHeight)
            {
                if (exists == false)
                {
                    ViewData["CategoryError"] = "The shelf does not mach the category";
                }
                if (book.Width > freeWidth)
                {
                    ViewData["WidthError"] = "The shelf does not have enough free space";

                }
                if (book.Height > freeHeight)
                {
                    ViewData["HeightError"] = "The shelf is not tall enough";
                }
                ViewData["ShelfId"] = new SelectList(releventShelves);
                ViewData["Categories"] = new SelectList(category);
                return View(book);
            }

            string message = "added sucessfuly";
            if (freeHeight - book.Height > 10)
            {
                message = "added sucessfuly Note that the book is lower than the shelf";
            }

            _context.Book.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Book" ,new { message = message});


            //ViewData["ShelfId"] = new SelectList(releventShelves);
            //return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["ShelfId"] = new SelectList(_context.Shelf, "ShelfId", "ShelfId", book.ShelfId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,BookName,BookCategory,Width,Height,ShelfId")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShelfId"] = new SelectList(_context.Shelf, "ShelfId", "ShelfId", book.ShelfId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }
    }
}
