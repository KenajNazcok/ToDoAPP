using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Controllers
{
    public class ToDoItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToDoItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ToDoItems
        public async Task<IActionResult> Index(string kategoria, string status)
        {
            var query = _context.ToDoItems.AsQueryable();

            if (!string.IsNullOrEmpty(kategoria) &&
                Enum.TryParse<KategoriaZadania>(kategoria, out var parsedKategoria))
            {
                query = query.Where(t => t.Kategoria == parsedKategoria);
            }

            if (!string.IsNullOrEmpty(status) &&
                Enum.TryParse<StatusZadania>(status, out var parsedStatus))
            {
                query = query.Where(t => t.Status == parsedStatus);
            }

            ViewData["KategoriaList"] = GetKategoriaList();
            ViewData["StatusList"] = GetStatusList();
            ViewBag.WybranaKategoria = kategoria;
            ViewBag.WybranyStatus = status;

            return View(await query.ToListAsync());
        }

        // GET: ToDoItems/Create
        public IActionResult Create()
        {
            ViewData["StatusList"] = GetStatusList();
            ViewData["KategoriaList"] = GetKategoriaList();
            return View();
        }

        // POST: ToDoItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Tytul,Opis,Status,Kategoria")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                toDoItem.DataUtworzenia = DateTime.Now;

                if (toDoItem.Status == StatusZadania.Zakonczone)
                    toDoItem.DataZrobienia = DateTime.Now;

                _context.Add(toDoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["StatusList"] = GetStatusList();
            ViewData["KategoriaList"] = GetKategoriaList();
            return View(toDoItem);
        }

        // GET: ToDoItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null) return NotFound();

            ViewData["StatusList"] = GetStatusList();
            ViewData["KategoriaList"] = GetKategoriaList();
            return View(toDoItem);
        }

        // POST: ToDoItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tytul,Opis,DataUtworzenia,DataZrobienia,Status,Kategoria")] ToDoItem toDoItem)
        {
            if (id != toDoItem.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (toDoItem.Status == StatusZadania.Zakonczone && toDoItem.DataZrobienia == null)
                        toDoItem.DataZrobienia = DateTime.Now;
                    else if (toDoItem.Status != StatusZadania.Zakonczone)
                        toDoItem.DataZrobienia = null;

                    _context.Update(toDoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoItemExists(toDoItem.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["StatusList"] = GetStatusList();
            ViewData["KategoriaList"] = GetKategoriaList();
            return View(toDoItem);
        }

        // GET: ToDoItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var toDoItem = await _context.ToDoItems.FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null) return NotFound();

            return View(toDoItem);
        }

        // POST: ToDoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem != null)
            {
                _context.ToDoItems.Remove(toDoItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoItemExists(int id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }

        private List<SelectListItem> GetStatusList()
        {
            return Enum.GetValues(typeof(StatusZadania))
                       .Cast<StatusZadania>()
                       .Select(s => new SelectListItem
                       {
                           Value = s.ToString(),
                           Text = s.ToString()
                       }).ToList();
        }

        private List<SelectListItem> GetKategoriaList()
        {
            return Enum.GetValues(typeof(KategoriaZadania))
                       .Cast<KategoriaZadania>()
                       .Select(k => new SelectListItem
                       {
                           Value = k.ToString(),
                           Text = k.ToString()
                       }).ToList();
        }
    }
}
