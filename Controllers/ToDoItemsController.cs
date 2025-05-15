using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;

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
		public async Task<IActionResult> Index()
		{
			var items = await _context.ToDoItems.ToListAsync();
			return View(items);
		}

		// GET: ToDoItems/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: ToDoItems/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Tytul,Opis,CzyZrobione")] ToDoItem toDoItem)
		{
			if (ModelState.IsValid)
			{
				_context.Add(toDoItem);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(toDoItem);
		}

		// GET: ToDoItems/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var toDoItem = await _context.ToDoItems.FindAsync(id);
			if (toDoItem == null) return NotFound();

			return View(toDoItem);
		}

		// POST: ToDoItems/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Tytul,Opis,CzyZrobione,DataZrobienia")] ToDoItem toDoItem)
		{
			if (id != toDoItem.Id) return NotFound();

			if (ModelState.IsValid)
			{
				try
				{
					// Ustaw dat� zrobienia, je�li zaznaczono jako zrobione i data nie jest ustawiona
					if (toDoItem.CzyZrobione && toDoItem.DataZrobienia == null)
					{
						toDoItem.DataZrobienia = DateTime.Now;
					}
					// Je�li zadanie nie jest zrobione, wyczy�� dat�
					else if (!toDoItem.CzyZrobione)
					{
						toDoItem.DataZrobienia = null;
					}

					_context.Update(toDoItem);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ToDoItemExists(toDoItem.Id))
						return NotFound();
					else
						throw;
				}
				return RedirectToAction(nameof(Index));
			}
			return View(toDoItem);
		}

		// GET: ToDoItems/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var toDoItem = await _context.ToDoItems
				.FirstOrDefaultAsync(m => m.Id == id);

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
	}
}
